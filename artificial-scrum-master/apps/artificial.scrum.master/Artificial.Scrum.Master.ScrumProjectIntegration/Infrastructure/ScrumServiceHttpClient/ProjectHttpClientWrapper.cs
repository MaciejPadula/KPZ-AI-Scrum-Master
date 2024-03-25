using System.Net;
using System.Text.Json;
using Artificial.Scrum.Master.ScrumProjectIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models;
using Artificial.Scrum.Master.ScrumProjectIntegration.Settings;
using Artificial.Scrum.Master.ScrumProjectIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ScrumServiceHttpClient
{
    public class ProjectHttpClientWrapper : IProjectHttpClientWrapper
    {
        private readonly ScrumManagementServiceSettings _settings;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJwtDecoder _jwtDecoder;
        private readonly IUserTokensRepository _userTokensRepository;

        public ProjectHttpClientWrapper(ScrumManagementServiceSettings settings, IHttpClientFactory httpClientFactory,
            IUserTokensRepository userTokensRepository, IJwtDecoder jwtDecoder)
        {
            _httpClientFactory = httpClientFactory;
            _userTokensRepository = userTokensRepository;
            _jwtDecoder = jwtDecoder;
            _settings = settings;
        }

        public async Task<WrapperHttpResponse> GetHttpRequest(string userId, string url)
        {
            var tokens = await _userTokensRepository.GetUserAccessTokens(userId);
            if (tokens is null)
            {
                throw new ProjectResourceNotFoundException($"Credentials of user:{userId} not found");
            }

            var isValid = ValidateAccessToken(tokens.Value.AccessToken);
            if (!isValid)
            {
                tokens = await RefreshUserTokens(userId, tokens.Value);
            }

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokens.Value.AccessToken}");

            var httpResponse = await httpClient.GetAsync(url);
            var result = await httpResponse.Content.ReadAsStringAsync();

            return new WrapperHttpResponse
            {
                Response = result,
                StatusCode = httpResponse.StatusCode
            };
        }

        public async Task<WrapperHttpResponse> PostHttpRequest(string userId, string url,
            Dictionary<string, string> parameters)
        {
            var tokens = await _userTokensRepository.GetUserAccessTokens(userId);
            if (tokens is null)
            {
                throw new ProjectResourceNotFoundException($"Credentials of user:{userId} not found");
            }

            var isValid = ValidateAccessToken(tokens.Value.AccessToken);
            if (!isValid)
            {
                tokens = await RefreshUserTokens(userId, tokens.Value);
            }

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokens.Value.AccessToken}");

            var content = new FormUrlEncodedContent(parameters);
            var httpResponse = await httpClient.PostAsync(url, content);
            var result = await httpResponse.Content.ReadAsStringAsync();

            return new WrapperHttpResponse
            {
                Response = result,
                StatusCode = httpResponse.StatusCode
            };
        }

        private bool ValidateAccessToken(string accessToken)
        {
            var expirationDate = _jwtDecoder.GetExpirationDate(accessToken, "exp");
            var expired = DateTime.UtcNow.AddMinutes(-(_settings.ExpirationTimeBuffer));
            return expirationDate >= expired;
        }

        private async Task<UserTokens> RefreshUserTokens(string userId, UserTokens currentUserTokens)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var values = new Dictionary<string, string>
            {
                { "refresh", currentUserTokens.RefreshToken }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await httpClient.PostAsync($"auth/refresh", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var serializedResponse = JsonSerializer.Deserialize<RefreshResponse>(responseString);

            if (response.StatusCode != HttpStatusCode.OK || serializedResponse is null)
            {
                throw new ProjectRequestFailedException(responseString);
            }

            var newTokens = new UserTokens
            {
                AccessToken = serializedResponse.AccessToken,
                RefreshToken = serializedResponse.RefreshToken
            };

            await _userTokensRepository.SaveUserAccessTokens(userId, newTokens);
            return newTokens;
        }
    }
}

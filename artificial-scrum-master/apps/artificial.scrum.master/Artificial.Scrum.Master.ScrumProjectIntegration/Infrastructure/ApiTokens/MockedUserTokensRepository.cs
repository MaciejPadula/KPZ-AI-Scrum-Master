using System.Net;
using System.Text.Json;
using Artificial.Scrum.Master.ScrumProjectIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models;
using Artificial.Scrum.Master.ScrumProjectIntegration.Settings;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens
{
    public class MockedUserTokensRepository : IUserTokensRepository
    {
        private readonly MockupTokenManager _mockupTokenManager;

        public MockedUserTokensRepository(ScrumManagementServiceSettings settings)
        {
            _mockupTokenManager = new MockupTokenManager(settings);
        }

        public async Task<UserTokens?> GetUserAccessTokens(string userId)
        {
            var loginResponse = _mockupTokenManager.LoginResponse ?? await _mockupTokenManager.LoginRequest();
            if (loginResponse is null)
            {
                throw new ProjectRequestFailedException("Failed to get access token");
            }

            return new UserTokens
            {
                AccessToken = loginResponse.AccessToken,
                RefreshToken = loginResponse.RefreshToken
            };
        }

        public Task SaveUserAccessTokens(string userId, UserTokens userTokens)
        {
            _mockupTokenManager.LoginResponse = new LoginResponse
            {
                AccessToken = userTokens.AccessToken,
                RefreshToken = userTokens.RefreshToken
            };

            return Task.CompletedTask;
        }
    }

    public class MockupTokenManager
    {
        // Replace Username and Password with your credentials 
        private static readonly string Type = "normal";
        private static readonly string Username = "";
        private static readonly string Password = "";

        private readonly ScrumManagementServiceSettings _settings;

        public LoginResponse? LoginResponse { get; set; }

        public MockupTokenManager(ScrumManagementServiceSettings settings)
        {
            _settings = settings;
        }

        public async Task<LoginResponse?> LoginRequest()
        {
            using var client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "type", Type },
                { "username", Username },
                { "password", Password }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync($"{_settings.BaseUrl}/auth", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var serializedResponse = JsonSerializer.Deserialize<LoginResponse>(responseString);

            if (response.StatusCode != HttpStatusCode.OK || serializedResponse is null)
            {
                throw new ProjectRequestFailedException(responseString);
            }

            LoginResponse = serializedResponse;
            return serializedResponse;
        }
    }
}

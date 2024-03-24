using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Artificial.Scrum.Master.ScrumProjectIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models;
using Artificial.Scrum.Master.ScrumProjectIntegration.Settings;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens
{
    public class MockedUserTokensRepository : IUserTokensRepository
    {
        // This is a mock implementation of the IUserTokensRepository interface.
        private readonly MockupTokenManager _mockupTokenManager;

        public MockedUserTokensRepository(MockupTokenManager mockupTokenManager)
        {
            _mockupTokenManager = mockupTokenManager;
        }

        public async Task<UserTokens> GetUserAccessToken(string userId)
        {
            var loginResponse = _mockupTokenManager.LoginResponse ?? await _mockupTokenManager.LoginRequest();
            if (loginResponse is null)
            {
                throw new RequestFailedException(HttpStatusCode.InternalServerError, "Failed to get access token");
            }

            return new UserTokens
            {
                AccessToken = loginResponse.AccessToken,
                RefreshToken = loginResponse.RefreshToken
            };
        }

        public Task SaveUserAccessToken(string userId, UserTokens userTokens)
        {
            _mockupTokenManager.LoginResponse = new LoginResponse
            {
                AccessToken = userTokens.AccessToken,
                RefreshToken = userTokens.RefreshToken
            };

            return Task.CompletedTask;
        }
    }

    public class LoginResponse
    {
        [JsonPropertyName("auth_token")] public string AccessToken { get; set; } = default!;
        [JsonPropertyName("refresh")] public string RefreshToken { get; set; } = default!;
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
                throw new RequestFailedException(response.StatusCode, responseString);
            }

            LoginResponse = serializedResponse;
            return serializedResponse;
        }
    }
}

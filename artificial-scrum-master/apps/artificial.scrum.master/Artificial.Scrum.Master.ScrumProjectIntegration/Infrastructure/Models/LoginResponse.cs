using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models;

public class LoginResponse
{
    [JsonPropertyName("refresh")] public required string RefreshToken { get; set; }
    [JsonPropertyName("auth_token")] public required string AccessToken { get; set; }
}

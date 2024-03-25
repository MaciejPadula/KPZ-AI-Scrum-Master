using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models
{
    public class RefreshResponse
    {
        [JsonPropertyName("auth_token")] public string AccessToken { get; set; } = default!;
        [JsonPropertyName("refresh")] public string RefreshToken { get; set; } = default!;
    }
}

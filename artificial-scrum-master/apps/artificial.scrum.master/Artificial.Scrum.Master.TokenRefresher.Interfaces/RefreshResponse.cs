using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.TokenRefresher;

public class RefreshResponse
{
    [JsonPropertyName("auth_token")]
    public required string AccessToken { get; set; }
    [JsonPropertyName("refresh")]
    public required string RefreshToken { get; set; }
}


using System.IdentityModel.Tokens.Jwt;

namespace Artificial.Scrum.Master.ScrumIntegration.Utilities;

internal class JwtDecoder : IJwtDecoder
{
    private readonly TimeProvider _timeProvider;
    private readonly JwtSecurityTokenHandler _handler;

    public JwtDecoder(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        _handler = new JwtSecurityTokenHandler();
    }

    public string? GetClaim(string token, string claimTypeName)
    {
        var jwtSecurityToken = _handler.ReadJwtToken(token);
        return jwtSecurityToken.Claims
            .FirstOrDefault(c => c.Type == claimTypeName)?.Value;
    }

    public DateTime? GetExpirationDate(string token, string claimTypeName)
    {
        var expirationStringValue = GetClaim(token, claimTypeName);
        if (string.IsNullOrEmpty(expirationStringValue))
        {
            return null;
        }

        var result = long.TryParse(expirationStringValue, out var expirationMilliseconds);
        if (!result)
        {
            return null;
        }

        return DateTimeOffset.FromUnixTimeSeconds(expirationMilliseconds).UtcDateTime;
    }
}

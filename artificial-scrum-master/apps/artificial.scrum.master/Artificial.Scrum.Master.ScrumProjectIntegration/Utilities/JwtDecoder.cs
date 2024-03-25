using System.IdentityModel.Tokens.Jwt;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Utilities
{
    public class JwtDecoder : IJwtDecoder
    {
        private readonly JwtSecurityTokenHandler _handler;

        public JwtDecoder()
        {
            _handler = new JwtSecurityTokenHandler();
        }

        public string? GetClaim(string token, string claimTypeName)
        {
            var jwtSecurityToken = _handler.ReadJwtToken(token);
            return jwtSecurityToken.Claims
                .FirstOrDefault(c => c.Type == claimTypeName)?.Value;
        }

        public DateTime GetExpirationDate(string token, string claimTypeName)
        {
            var expirationStringValue = GetClaim(token, claimTypeName);
            if (expirationStringValue is null)
            {
                return DateTime.UtcNow;
            }

            var result = long.TryParse(expirationStringValue, out var expirationMilliseconds);
            if (!result)
            {
                return DateTime.UtcNow;
            }

            return DateTimeOffset.FromUnixTimeSeconds(expirationMilliseconds).UtcDateTime;
        }
    }
}

using System.IdentityModel.Tokens.Jwt;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Utilities
{
    public class JwtDecoder
    {
        public static string Decode(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken.ToString();
        }

        public static string GetClaim(string token, string claimType)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }
}

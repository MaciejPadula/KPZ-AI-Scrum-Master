using Artificial.Scrum.Master.ScrumProjectIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens
{
    public class TokenValidator : ITokenValidator
    {
        private const int TokenRefreshRequestTimeBuffer = 2;
        private const string ExpirationTimeClaimTypeName = "exp";

        private readonly IJwtDecoder _jwtDecoder;

        public TokenValidator(IJwtDecoder jwtDecoder)
        {
            _jwtDecoder = jwtDecoder;
        }

        public bool ValidateAccessTokenExpirationTime(string accessToken)
        {
            var expirationDate = _jwtDecoder.GetExpirationDate(accessToken, ExpirationTimeClaimTypeName);
            var expired = DateTime.UtcNow.AddMinutes(-TokenRefreshRequestTimeBuffer);
            return expirationDate >= expired;
        }
    }
}

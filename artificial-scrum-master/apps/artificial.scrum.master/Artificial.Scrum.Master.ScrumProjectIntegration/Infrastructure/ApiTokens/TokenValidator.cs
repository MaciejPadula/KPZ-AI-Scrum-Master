using Artificial.Scrum.Master.ScrumProjectIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens
{
    public class TokenValidator : ITokenValidator
    {
        private const int TokenRefreshRequestTimeBuffer = 2;
        private const string ExpirationTimeClaimTypeName = "exp";

        private readonly TimeProvider _timeProvider;
        private readonly IJwtDecoder _jwtDecoder;

        public TokenValidator(TimeProvider timeProvider, IJwtDecoder jwtDecoder)
        {
            _timeProvider = timeProvider;
            _jwtDecoder = jwtDecoder;
        }

        public bool ValidateAccessTokenExpirationTime(string accessToken)
        {
            var expirationDate = _jwtDecoder.GetExpirationDate(accessToken, ExpirationTimeClaimTypeName);
            var expired = _timeProvider.GetUtcNow().AddMinutes(-TokenRefreshRequestTimeBuffer).UtcDateTime;
            return expirationDate >= expired;
        }
    }
}

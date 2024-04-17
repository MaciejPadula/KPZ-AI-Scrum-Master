using Artificial.Scrum.Master.ScrumIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;

internal class TokenValidator : ITokenValidator
{
    private const int TokenRefreshRequestTimeBuffer = 5;
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

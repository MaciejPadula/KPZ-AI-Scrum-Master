namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;

internal interface ITokenValidator
{
    bool ValidateAccessTokenExpirationTime(string accessToken);
}

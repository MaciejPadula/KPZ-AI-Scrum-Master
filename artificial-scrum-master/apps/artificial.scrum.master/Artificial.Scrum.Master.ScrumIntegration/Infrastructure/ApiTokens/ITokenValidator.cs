namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;

public interface ITokenValidator
{
    bool ValidateAccessTokenExpirationTime(string accessToken);
}

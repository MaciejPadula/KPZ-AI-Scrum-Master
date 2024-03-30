namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens;

public interface ITokenValidator
{
    bool ValidateAccessTokenExpirationTime(string accessToken);
}

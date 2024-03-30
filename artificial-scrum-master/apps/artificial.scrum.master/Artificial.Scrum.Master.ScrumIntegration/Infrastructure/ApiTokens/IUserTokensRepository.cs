namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;

public interface IUserTokensRepository
{
    Task<string?> GetUserAccessTokens(string userId);
    Task SaveAccessTokens(string userId, string tokens);
}

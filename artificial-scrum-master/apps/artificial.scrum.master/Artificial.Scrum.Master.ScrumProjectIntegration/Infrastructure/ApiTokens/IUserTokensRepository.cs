namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens;

public interface IUserTokensRepository
{
    Task<string?> GetUserAccessTokens(string userId);
    Task SaveAccessTokens(string userId, string tokens);
}
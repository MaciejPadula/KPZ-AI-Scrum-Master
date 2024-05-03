using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;

public interface IUserTokensRepository
{
    Task<UserTokens?> GetUserAccessTokens(string userId);
    Task<List<UserTokens>> GetAllAccessTokens();
    Task SaveAccessTokensWhenExists(string userId, UserTokens tokens);
}

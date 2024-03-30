using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens
{
    public interface IUserTokensRepository
    {
        Task<UserTokens?> GetUserAccessTokens(string userId);
        Task SaveUserAccessTokens(string userId, UserTokens userTokens);
    }
}

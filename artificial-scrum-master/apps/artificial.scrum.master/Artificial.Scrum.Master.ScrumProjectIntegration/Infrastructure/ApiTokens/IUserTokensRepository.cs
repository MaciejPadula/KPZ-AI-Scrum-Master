using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens
{
    public interface IUserTokensRepository
    {
        Task<UserTokens> GetUserAccessToken(string userId);
        Task SaveUserAccessToken(string userId, UserTokens userTokens);
    }
}

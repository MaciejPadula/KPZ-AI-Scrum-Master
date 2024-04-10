using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

namespace Artificial.Scrum.Master.Infrastructure.Repositories;
internal class SqlSessionUserRepository : ISessionUserRepository
{
    public Task AddSessionUser(SessionUserEntity user)
    {
        throw new NotImplementedException();
    }

    public Task<List<SessionUserEntity>> GetSessionUsers(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UserExists(int userId)
    {
        throw new NotImplementedException();
    }
}

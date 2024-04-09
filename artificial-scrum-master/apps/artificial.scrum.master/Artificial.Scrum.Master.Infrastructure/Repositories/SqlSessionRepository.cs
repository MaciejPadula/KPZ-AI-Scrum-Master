using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

namespace Artificial.Scrum.Master.Infrastructure.Repositories;

internal class SqlSessionRepository : ISessionRepository
{
    public Task AddSessionTask(SessionTaskEntity task)
    {
        throw new NotImplementedException();
    }

    public Task AddSessionUser(SessionUserEntity user)
    {
        throw new NotImplementedException();
    }

    public Task<List<SessionUserEntity>> GetSessionUsers(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Task<List<SessionEntity>> GetUserProjectSessions(string userId, int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SessionExists(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateUserAccess(string userId, string sessionId)
    {
        throw new NotImplementedException();
    }
}

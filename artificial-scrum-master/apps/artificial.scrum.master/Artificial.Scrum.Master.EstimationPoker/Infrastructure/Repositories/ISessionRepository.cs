using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

public interface ISessionRepository
{
    Task<List<SessionEntity>> GetUserProjectSessions(string userId, int projectId);
    Task AddSessionUser(SessionUserEntity user);
    Task<List<SessionUserEntity>> GetSessionUsers(string sessionId);
    Task AddSessionTask(SessionTaskEntity task);
    Task<bool> ValidateUserAccess(string userId, string sessionId);
    Task<bool> SessionExists(string sessionId);
}

using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

public interface ISessionRepository
{
    Task AddSession(SessionEntity session);
    Task<List<SessionEntity>> GetUserProjectSessions(string userId, int projectId);
    Task<SessionEntity?> GetSession(string sessionId);
    Task<bool> SessionExists(string sessionId);
    Task<bool> ValidateUserAccess(string userId, string sessionId);
}

using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

public interface ISessionUserRepository
{
    Task AddSessionUser(SessionUserEntity user);
    Task<List<SessionUserEntity>> GetSessionUsers(string sessionId);

    Task<bool> UserExists(int userId);
}

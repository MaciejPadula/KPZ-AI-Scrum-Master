namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

public interface IActiveUserRepository
{
    bool RegisterUser(string sessionId, string username);
    HashSet<string> GetUsers(string sessionId);
}

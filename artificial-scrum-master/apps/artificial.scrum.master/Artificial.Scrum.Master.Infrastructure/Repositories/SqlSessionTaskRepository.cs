using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

namespace Artificial.Scrum.Master.Infrastructure.Repositories;
internal class SqlSessionTaskRepository : ISessionTaskRepository
{
    public Task AddSessionTask(SessionTaskEntity task)
    {
        throw new NotImplementedException();
    }

    public Task AddTaskEstimation(SessionTaskEstimationEntity estimation)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EstimationExists(string sessionId, int userId, int taskId)
    {
        throw new NotImplementedException();
    }

    public Task<SessionTaskEntity?> GetLatestTask(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Task<List<SessionTaskEstimationEntity>> GetTaskEstimations(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> TaskExists(int taskId)
    {
        throw new NotImplementedException();
    }
}

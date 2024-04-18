using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

public interface ISessionTaskRepository
{
    Task AddSessionTask(SessionTaskEntity task);
    Task AddTaskEstimation(SessionTaskEstimationEntity estimation);
    Task<List<SessionTaskEstimationEntity>> GetTaskEstimations(int taskId);
    Task<SessionTaskEntity?> GetLatestTask(string sessionId);

    Task<bool> TaskExists(int taskId);
    Task<bool> EstimationExists(string username, int taskId);
}

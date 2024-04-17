using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using Dapper;

namespace Artificial.Scrum.Master.Infrastructure.Repositories;
internal class SqlSessionTaskRepository : ISessionTaskRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SqlSessionTaskRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task AddSessionTask(SessionTaskEntity task)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        await connection.ExecuteAsync(@"
INSERT INTO [EstimationPoker].[SessionTasks]
(Title, SessionId, Description, CreatedAt)
VALUES
(@Title, @SessionId, @Description, @CreatedAt)
", new { task.Title, task.SessionId, task.Description, task.CreatedAt });
    }

    public async Task AddTaskEstimation(SessionTaskEstimationEntity estimation)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        await connection.ExecuteAsync(@"
INSERT INTO [EstimationPoker].[TaskEstimations]
(TaskId, Username, Value)
VALUES
(@TaskId, @Username, @Value)
", new { estimation.TaskId, estimation.Username, estimation.Value });
    }

    public async Task<bool> EstimationExists(string username, int taskId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        return await connection.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM [EstimationPoker].[TaskEstimations]
WHERE TaskId = @TaskId AND Username = @Username
", new { taskId, username });
    }

    public async Task<SessionTaskEntity?> GetLatestTask(string sessionId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryFirstOrDefaultAsync<SessionTaskEntity>($@"
SELECT
    Id AS {nameof(SessionTaskEntity.Id)},
    Title AS {nameof(SessionTaskEntity.Title)},
    Description AS {nameof(SessionTaskEntity.Description)},
    SessionId AS {nameof(SessionTaskEntity.SessionId)},
    CreatedAt AS {nameof(SessionTaskEntity.CreatedAt)}
FROM [EstimationPoker].[SessionTasks]
WHERE SessionId = @SessionId
ORDER BY CreatedAt DESC
", new { sessionId });

        return result == default ? null : result;
    }

    public async Task<List<SessionTaskEstimationEntity>> GetTaskEstimations(int taskId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryAsync<SessionTaskEstimationEntity>($@"
SELECT
    TaskId AS {nameof(SessionTaskEstimationEntity.TaskId)},
    Username AS {nameof(SessionTaskEstimationEntity.Username)},
    Value AS {nameof(SessionTaskEstimationEntity.Value)}
FROM [EstimationPoker].[TaskEstimations]
WHERE TaskId = @TaskId
", new { taskId });

        return result.ToList();
    }

    public async Task<bool> TaskExists(int taskId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        return await connection.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM [EstimationPoker].[SessionTasks]
WHERE Id = @TaskId
", new { taskId });
    }
}

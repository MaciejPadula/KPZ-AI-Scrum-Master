using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using Dapper;

namespace Artificial.Scrum.Master.Infrastructure.Repositories;

internal class SqlSessionRepository : ISessionRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SqlSessionRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task AddSession(SessionEntity session)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        await connection.ExecuteAsync(@"
INSERT INTO [EstimationPoker].[Sessions]
(Id, UserId, ProjectId, Name)
VALUES
(@Id, @OwnerId, @ProjectId, @Name)
", new { session.Id, session.OwnerId, session.ProjectId, session.Name });
    }

    public async Task<SessionEntity?> GetSession(string sessionId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryFirstOrDefaultAsync<SessionEntity>($@"
SELECT TOP 1
    Id AS {nameof(SessionEntity.Id)},
    Name AS {nameof(SessionEntity.Name)},
    UserId AS {nameof(SessionEntity.OwnerId)},
    ProjectId AS {nameof(SessionEntity.ProjectId)}
FROM [EstimationPoker].[Sessions]
WHERE Id = @SessionId
", new { sessionId });

        return result == default ? null : result;
    }

    public async Task<List<SessionEntity>> GetUserProjectSessions(string userId, int projectId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryAsync<SessionEntity>($@"
SELECT
    Id AS {nameof(SessionEntity.Id)},
    Name AS {nameof(SessionEntity.Name)},
    UserId AS {nameof(SessionEntity.OwnerId)},
    ProjectId AS {nameof(SessionEntity.ProjectId)}
FROM [EstimationPoker].[Sessions]
WHERE UserId = @UserId AND ProjectId = @ProjectId
", new { userId, projectId });

        return result.ToList();
    }

    public async Task<bool> SessionExists(string sessionId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        return await connection.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM [EstimationPoker].[Sessions]
WHERE Id = @SessionId
", new { sessionId });
    }

    public async Task<bool> ValidateUserAccess(string userId, string sessionId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        return await connection.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM [EstimationPoker].[Sessions]
WHERE Id = @SessionId AND UserId = @UserId
", new { sessionId, userId });
    }
}

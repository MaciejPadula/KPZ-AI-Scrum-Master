using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using Dapper;

namespace Artificial.Scrum.Master.Infrastructure.Repositories;

internal class SqlRetroSessionRepository : IRetroSessionRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SqlRetroSessionRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task CreateSession(RetroSession session)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        await connection.ExecuteAsync(@"
INSERT INTO [Retrospectives].[Sessions]
(Id, Name, SprintId, ProjectId)
VALUES (@Id, @Name, @SprintId, @ProjectId)
", new { session.Id, session.Name, session.SprintId, session.ProjectId });
    }

    public async Task CreateSessionCard(SessionCard sessionCard)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        await connection.ExecuteAsync(@"
INSERT INTO [Retrospectives].[Cards]
(Content, Type, SessionId, CreatedAt)
VALUES
(@Content, @Type, @SessionId, @CreatedAt)
", new { sessionCard.Content, sessionCard.Type, sessionCard.SessionId, sessionCard.CreatedAt });
    }

    public async Task<RetroSession?> GetSession(int sprintId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryFirstOrDefaultAsync<RetroSession>(@$"
SELECT
    Id AS [{nameof(RetroSession.Id)}],
    Name AS [{nameof(RetroSession.Name)}],
    SprintId AS [{nameof(RetroSession.SprintId)}],
    ProjectId AS [{nameof(RetroSession.ProjectId)}]
FROM [Retrospectives].[Sessions]
WHERE SprintId = @SprintId
", new { sprintId });

        return result == default ? null : result;
    }

    public async Task<RetroSession?> GetSession(string sessionId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryFirstOrDefaultAsync<RetroSession>(@$"
SELECT
    Id AS [{nameof(RetroSession.Id)}],
    Name AS [{nameof(RetroSession.Name)}],
    SprintId AS [{nameof(RetroSession.SprintId)}],
    ProjectId AS [{nameof(RetroSession.ProjectId)}]
FROM [Retrospectives].[Sessions]
WHERE Id = @SessionId
", new { sessionId });

        return result == default ? null : result;
    }

    public async Task<List<SessionCard>> GetSessionCards(string sessionId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryAsync<SessionCard>(@$"
SELECT
    Content AS [{nameof(SessionCard.Content)}],
    Type AS [{nameof(SessionCard.Type)}],
    SessionId AS [{nameof(SessionCard.SessionId)}],
    CreatedAt AS [{nameof(SessionCard.CreatedAt)}]
FROM [Retrospectives].[Cards]
WHERE SessionId = @SessionId
ORDER BY CreatedAt DESC
", new { sessionId });

        return result.ToList();
    }

    public async Task<bool> SessionExists(string sessionId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        return await connection.ExecuteScalarAsync<bool>(@$"
SELECT 1
FROM [Retrospectives].[Sessions]
WHERE Id = @SessionId
", new { sessionId });
    }
}

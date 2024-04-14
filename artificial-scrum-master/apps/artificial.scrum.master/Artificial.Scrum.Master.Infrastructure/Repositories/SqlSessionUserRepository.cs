using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using Dapper;

namespace Artificial.Scrum.Master.Infrastructure.Repositories;
internal class SqlSessionUserRepository : ISessionUserRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SqlSessionUserRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task AddSessionUser(SessionUserEntity user)
    {
        using var connection = _dbConnectionFactory.GetOpenConnection();

        await connection.ExecuteAsync($@"
INSERT INTO [EstimationPoker].[SessionUsers]
(UserName, SessionId)
VALUES
(@UserName, @SessionId)
", new { user.UserName, user.SessionId });
    }

    public async Task<List<SessionUserEntity>> GetSessionUsers(string sessionId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryAsync<SessionUserEntity>($@"
SELECT
    Id AS [{nameof(SessionUserEntity.Id)}],
    UserName AS [{nameof(SessionUserEntity.UserName)}],
    SessionId AS [{nameof(SessionUserEntity.SessionId)}]
FROM [EstimationPoker].[SessionUsers]
WHERE SessionId = @SessionId
", new { sessionId });

        return result.ToList();
    }

    public async Task<bool> UserExists(int userId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        return await connection.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM [EstimationPoker].[SessionUsers]
WHERE Id = @UserId
", new { userId });
    }
}

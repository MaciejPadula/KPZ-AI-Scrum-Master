using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;
using Dapper;

namespace Artificial.Scrum.Master.Infrastructure.Repositories;

internal class SqlUserSettingsRepository : IUserSettingsRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SqlUserSettingsRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task AddUserSettings(UserSettingsEntity userSettings)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        await connection.ExecuteAsync(@"
INSERT INTO [ScrumMaster].[UserSettings]
(UserId, TaigaApiKey)
VALUES
(@UserId, @TaigaApiKey)
", new { userSettings.UserId, userSettings.TaigaApiKey });
    }

    public async Task<UserSettingsEntity> GetUserSettings(string userId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryFirstOrDefaultAsync<UserSettingsEntity>(@$"
SELECT TOP 1
    UserId AS {nameof(UserSettingsEntity.UserId)},
    TaigaApiKey AS {nameof(UserSettingsEntity.TaigaApiKey)}
FROM [ScrumMaster].[UserSettings]
WHERE UserId = @userId
", new { userId });

        return result;
    }

    public async Task UpdateUserSettings(UserSettingsEntity userSettings)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        await connection.ExecuteAsync(@"
UPDATE [ScrumMaster].[UserSettings]
SET TaigaApiKey = @TaigaApiKey
WHERE UserId = @UserId
", new { userSettings.UserId, userSettings.TaigaApiKey });
    }

    public async Task<bool> UserSettingsExists(string userId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM [ScrumMaster].[UserSettings]
WHERE UserId = @userId", new { userId });

        return result;
    }
}

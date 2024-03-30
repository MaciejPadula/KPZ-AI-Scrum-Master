using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;
using Dapper;

namespace Artificial.Scrum.Master.Infrastructure.Repositories;

internal class SqlUserSettingsRepository : IUserSettingsRepository, IUserTokensRepository
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
(UserId, TaigaAccess)
VALUES
(@UserId, @TaigaAccess)
", new { userSettings.UserId, userSettings.TaigaAccess });
    }

    public async Task<UserSettingsEntity?> GetUserSettings(string userId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryFirstOrDefaultAsync<UserSettingsEntity>(@$"
SELECT TOP 1
    UserId AS {nameof(UserSettingsEntity.UserId)},
    TaigaAccess AS {nameof(UserSettingsEntity.TaigaAccess)}
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
SET TaigaAccess = @TaigaAccess
WHERE UserId = @UserId
", new { userSettings.UserId, userSettings.TaigaAccess });
    }

    public async Task<string?> GetUserAccessTokens(string userId)
    {
        var result = await GetUserSettings(userId);

        return result?.TaigaAccess;
    }

    public async Task SaveAccessTokens(string userId, string tokens)
    {
        var settings = await GetUserSettings(userId);

        if (settings is null)
        {
            await AddUserSettings(new UserSettingsEntity(
                userId,
                tokens
            ));
        }
        else
        {
            await UpdateUserSettings(settings with
            {
                TaigaAccess = tokens
            });
        }
    }
}

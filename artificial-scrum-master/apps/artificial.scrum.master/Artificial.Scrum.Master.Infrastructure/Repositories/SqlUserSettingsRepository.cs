using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;
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
(UserId, TaigaAccessToken, TaigaRefreshToken)
VALUES
(@UserId, @TaigaAccessToken, @TaigaRefreshToken)
", new { userSettings.UserId, userSettings.TaigaAccessToken, userSettings.TaigaRefreshToken});
    }

    public async Task<UserTokens?> GetUserAccessTokens(string userId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryFirstOrDefaultAsync<UserTokens>(@$"
SELECT TOP 1
    UserId AS {nameof(UserSettingsEntity.UserId)},
    TaigaAccessToken AS {nameof(UserTokens.AccessToken)},
    TaigaRefreshToken AS {nameof(UserTokens.RefreshToken)}
FROM [ScrumMaster].[UserSettings]
WHERE UserId = @userId
", new { userId });

        return result;
    }

    public async Task<UserSettingsEntity?> GetUserSettings(string userId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryFirstOrDefaultAsync<UserSettingsEntity>(@$"
SELECT TOP 1
    UserId AS {nameof(UserSettingsEntity.UserId)},
    TaigaAccessToken AS {nameof(UserSettingsEntity.TaigaAccessToken)},
    TaigaRefreshToken AS {nameof(UserSettingsEntity.TaigaRefreshToken)}
FROM [ScrumMaster].[UserSettings]
WHERE UserId = @userId
", new { userId });

        return result;
    }

    public async Task SaveAccessTokensWhenExists(string userId, UserTokens tokens)
    {
        var settings = await GetUserSettings(userId);
        if (settings is null)
        {
            return;
        }

        await UpdateUserSettings(settings with
        {
            TaigaAccessToken = tokens.AccessToken,
            TaigaRefreshToken = tokens.RefreshToken
        });
    }

    public async Task UpdateUserSettings(UserSettingsEntity userSettings)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        await connection.ExecuteAsync(@"
UPDATE [ScrumMaster].[UserSettings]
SET
    TaigaAccessToken = @TaigaAccessToken,
    TaigaRefreshToken = @TaigaRefreshToken
WHERE UserId = @UserId
", new { userSettings.UserId, userSettings.TaigaAccessToken, userSettings.TaigaRefreshToken});
    }
}

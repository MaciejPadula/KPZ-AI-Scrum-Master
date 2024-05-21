using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;
using Artificial.Scrum.Master.TokenRefresher;
using Artificial.Scrum.Master.TokenRefresher.Interfaces;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;
using Dapper;

namespace Artificial.Scrum.Master.Infrastructure.Repositories;

internal class SqlUserSettingsRepository : IUserSettingsRepository, IUserTokensRepository, ITokenRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    private const int BatchSize = 2000;

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
", new { userSettings.UserId, userSettings.TaigaAccessToken, userSettings.TaigaRefreshToken });
    }

    public async Task<UserTokens?> GetUserAccessTokens(string userId)
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = await connection.QueryFirstOrDefaultAsync<UserTokens>(@$"
SELECT TOP 1
    UserId AS {nameof(UserTokens.UserId)},
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
", new { userSettings.UserId, userSettings.TaigaAccessToken, userSettings.TaigaRefreshToken });
    }

    public async Task<Dictionary<string, string>> GetAllAccessTokens()
    {
        using var connection = await _dbConnectionFactory.GetOpenConnectionAsync();

        var result = new List<UserTokens>();
        List<UserTokens> currentTokens;
        var page = 0;

        do
        {
            var response = await connection.QueryAsync<UserTokens>(@$"
SELECT
    UserId AS {nameof(UserTokens.UserId)},
    TaigaAccessToken AS {nameof(UserTokens.AccessToken)},
    TaigaRefreshToken AS {nameof(UserTokens.RefreshToken)}
FROM [ScrumMaster].[UserSettings]
ORDER BY UserId
OFFSET @Offset ROWS
FETCH NEXT @BatchSize ROWS ONLY
", new { BatchSize, Offset = BatchSize * page });

            currentTokens = response.ToList();
            result.AddRange(currentTokens);
            page += 1;
        }
        while (currentTokens.Count != 0);

        return result
            .ToDictionary(
                x => x.UserId,
                x => x.RefreshToken);
    }

    public async Task UpdateToken(string userId, RefreshResponse result) =>
        await SaveAccessTokensWhenExists(
            userId,
            new UserTokens(userId, result.AccessToken, result.RefreshToken));
}

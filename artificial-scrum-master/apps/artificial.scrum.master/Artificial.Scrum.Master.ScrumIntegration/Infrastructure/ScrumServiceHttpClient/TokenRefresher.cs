using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

internal interface ITokenRefresher
{
    Task<UserTokens> RefreshUserTokens(string userId, string refreshToken);
}

internal class TokenRefresher : ITokenRefresher
{
    private readonly IUserTokensRepository _userTokensRepository;
    private readonly ITokenValidator _tokenValidator;
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;

    private const string RefreshTokenUrl = "auth/refresh";
    private readonly TimeSpan CacheTTL = TimeSpan.FromMinutes(20);

    public TokenRefresher(
        IUserTokensRepository userTokensRepository,
        ITokenValidator tokenValidator,
        HttpClient httpClient,
        IMemoryCache memoryCache)
    {
        _userTokensRepository = userTokensRepository;
        _tokenValidator = tokenValidator;
        _httpClient = httpClient;
        _memoryCache = memoryCache;
    }

    public async Task<UserTokens> RefreshUserTokens(string userId, string refreshToken) =>
        await _memoryCache.GetOrCreateAsync(
            $"{nameof(RefreshUserTokens)}_{userId}",
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheTTL;
                return await RefreshUserTokensInternal(userId, refreshToken);
            }) ?? throw new RefreshTokenExpiredException("Refresh token is expired, please log in");

    private async Task<UserTokens> RefreshUserTokensInternal(string userId, string refreshToken)
    {
        if (!_tokenValidator.ValidateAccessTokenExpirationTime(refreshToken))
        {
            throw new RefreshTokenExpiredException("Refresh token is expired, please log in");
        }

        var httpResponse = await _httpClient.PostAsJsonAsync(RefreshTokenUrl, new { refresh = refreshToken });
        await EnsureStatusSuccess(httpResponse);

        var result = await httpResponse.Content.ReadFromJsonAsync<RefreshResponse>()
                     ?? throw new ProjectRequestFailedException("Response deserialization failed");

        var newTokens = new UserTokens(
            result.AccessToken,
            result.RefreshToken);

        await _userTokensRepository.SaveAccessTokensWhenExists(userId, newTokens);
        return newTokens;
    }

    private static async Task EnsureStatusSuccess(HttpResponseMessage httpResponse)
    {
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync();
            throw new ProjectRequestFailedException(
                $"Request failed with status code {httpResponse.StatusCode}: {errorContent}");
        }
    }
}

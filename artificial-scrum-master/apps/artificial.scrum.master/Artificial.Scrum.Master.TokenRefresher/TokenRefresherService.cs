using Artificial.Scrum.Master.TokenRefresher.Interfaces;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Polly;
using System.Net.Http.Json;

namespace Artificial.Scrum.Master.TokenRefresher;

internal interface ITokenRefresherService
{
    Task Execute();
}

internal class TokenRefresherService : ITokenRefresherService
{
    private readonly ILogger<TokenRefresherService> _logger;
    private readonly TelemetryClient _telemetryClient;
    private readonly ITokenRepository _userTokenRepository;
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy _throttlingPolicy;

    private const int RetryCount = 5;
    private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan RequestDelay = TimeSpan.FromMinutes(1);

    public TokenRefresherService(
        ILogger<TokenRefresherService> logger,
        TelemetryClient telemetryClient,
        ITokenRepository userTokenRepository,
        HttpClient httpClient)
    {
        _logger = logger;
        _telemetryClient = telemetryClient;
        _userTokenRepository = userTokenRepository;
        _httpClient = httpClient;
        _throttlingPolicy = Policy
            .Handle<ThrottledException>()
            .WaitAndRetryAsync(
                RetryCount,
                _ => RetryDelay);
    }

    public async Task Execute()
    {
        using var operation = _telemetryClient.StartOperation<RequestTelemetry>("TokenRefreshing");
        var tokens = await _userTokenRepository.GetAllAccessTokens();
        var userIds = new List<string>();

        foreach (var token in tokens)
        {
            await TryRefreshToken(token.Key, token.Value);

            if (!string.IsNullOrEmpty(token.Key))
            {
                userIds.Add(token.Key);
            }

            await Task.Delay(RequestDelay);
        }

        _telemetryClient.TrackEvent("TokensRefreshed", new Dictionary<string, string>
        {
            { "UserIds", $"[{string.Join(",", userIds)}]" },
            { "ProcessedTokens", userIds.Count.ToString() }
        });
    }

    private async Task<string?> TryRefreshToken(string userId, string refreshToken)
    {
        try
        {
            return await _throttlingPolicy.ExecuteAsync(() => RefreshToken(userId, refreshToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refresh token for user {userId}", userId);
            return null;
        }
    }

    private async Task<string?> RefreshToken(string userId, string refreshToken)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/refresh", new { refresh = refreshToken });
        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            throw new ThrottledException();
        }

        var result = await response.Content.ReadFromJsonAsync<RefreshResponse>();
        if (result is null)
        {
            _logger.LogError("Failed to obtain token for user {UserId}", userId);
            return null;
        }

        await _userTokenRepository.UpdateToken(userId, result);
        return userId;
    }
}

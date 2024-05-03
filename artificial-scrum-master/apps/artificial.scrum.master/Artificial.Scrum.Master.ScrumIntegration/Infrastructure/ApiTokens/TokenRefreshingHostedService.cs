using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Hosting;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;

internal class TokenRefreshingHostedService : IHostedService
{
    private readonly PeriodicTimer _timer;
    private static readonly TimeSpan _refreshTimeout = TimeSpan.FromHours(24);
    private readonly ITokenRefresher _tokenRefresher;
    private readonly IUserTokensRepository _userTokensRepository;
    private readonly TelemetryClient _telemetryClient;

    private const int BatchSize = 2000;

    public TokenRefreshingHostedService(
        ITokenRefresher tokenRefresher,
        IUserTokensRepository userTokensRepository,
        TelemetryClient telemetryClient)
    {
        _timer = new PeriodicTimer(_refreshTimeout);
        _tokenRefresher = tokenRefresher;
        _userTokensRepository = userTokensRepository;
        _telemetryClient = telemetryClient;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => HandleRefresh(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    private async Task HandleRefresh(CancellationToken cancellationToken)
    {
        while (await _timer.WaitForNextTickAsync(cancellationToken))
        {
            using var operation = _telemetryClient.StartOperation<RequestTelemetry>("TokenRefreshing");
            var tokens = await _userTokensRepository.GetAllAccessTokens();

            foreach (var tokensChunk in tokens.Chunk(BatchSize))
            {
                var tasks = tokensChunk
                    .Select(token =>
                        _tokenRefresher.RefreshUserTokens(
                            token.UserId,
                            token.RefreshToken))
                    .ToList();

                await Task.WhenAll(tasks);

                _telemetryClient.TrackEvent("TokensRefreshed", new Dictionary<string, string>
                {
                    { "UserIds", $"[{string.Join(",", tokensChunk.Select(x => x.UserId))}]" },
                    { "ProcessedTokens", tasks.Count.ToString() }
                });
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

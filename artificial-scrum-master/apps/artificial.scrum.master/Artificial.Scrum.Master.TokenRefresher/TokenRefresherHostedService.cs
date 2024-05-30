namespace Artificial.Scrum.Master.TokenRefresher;

public class TokenRefresherHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public TokenRefresherHostedService(
        IServiceScopeFactory serviceScopeFactory,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var tokenRefresherService = scope.ServiceProvider.GetRequiredService<ITokenRefresherService>();

        await tokenRefresherService.Execute(stoppingToken);
        _hostApplicationLifetime.StopApplication();
    }
}

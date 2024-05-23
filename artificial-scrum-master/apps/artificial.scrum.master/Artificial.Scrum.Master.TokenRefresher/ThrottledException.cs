namespace Artificial.Scrum.Master.TokenRefresher;

internal class ThrottledException : Exception
{
    private const int AdditionalThrottleDelayInSeconds = 5;
    public TimeSpan ThrottleDelay { get; private set; }

    public ThrottledException(int throttleDelayInSeconds)
    {
        ThrottleDelay = TimeSpan.FromSeconds(throttleDelayInSeconds + AdditionalThrottleDelayInSeconds);
    }
}

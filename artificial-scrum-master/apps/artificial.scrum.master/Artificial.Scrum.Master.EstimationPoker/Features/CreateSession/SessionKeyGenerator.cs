namespace Artificial.Scrum.Master.EstimationPoker.Features.CreateSession;

internal interface ISessionKeyGenerator
{
    string Key { get; }
}

internal class SessionKeyGenerator : ISessionKeyGenerator
{
    public string Key => Guid.NewGuid().ToString().Replace("-", "");
}

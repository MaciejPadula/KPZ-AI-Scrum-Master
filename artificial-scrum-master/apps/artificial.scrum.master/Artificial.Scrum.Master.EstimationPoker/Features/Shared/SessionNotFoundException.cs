namespace Artificial.Scrum.Master.EstimationPoker.Features.Shared;

internal class SessionNotFoundException : Exception
{
    public string SessionId { get; private set; }

    public SessionNotFoundException(string sessionId)
    {
        SessionId = sessionId;
    }

    public override string Message => $"Session with id {SessionId} not found";
}

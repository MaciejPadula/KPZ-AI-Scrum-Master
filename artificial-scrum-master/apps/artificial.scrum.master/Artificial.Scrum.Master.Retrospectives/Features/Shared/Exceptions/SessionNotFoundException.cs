namespace Artificial.Scrum.Master.Retrospectives.Features.Shared.Exceptions;

internal class SessionNotFoundException : Exception
{
    public SessionNotFoundException(string sessionId) : base($"Session with id {sessionId} not found")
    {
    }
}

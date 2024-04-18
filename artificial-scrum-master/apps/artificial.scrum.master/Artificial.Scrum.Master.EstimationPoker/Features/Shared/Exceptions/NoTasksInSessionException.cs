namespace Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
internal class NoTasksInSessionException : Exception
{
    public NoTasksInSessionException(string sessionId) : base($"No tasks in session {sessionId}")
    {
    }
}

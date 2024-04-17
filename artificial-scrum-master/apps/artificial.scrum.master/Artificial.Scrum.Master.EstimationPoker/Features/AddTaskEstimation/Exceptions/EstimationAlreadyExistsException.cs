namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Exceptions;

internal class EstimationAlreadyExistsException : Exception
{
    public string SessionId { get; private set; }
    public string Username { get; private set; }
    public int TaskId { get; private set; }

    public EstimationAlreadyExistsException(string sessionId, string username, int taskId)
    {
        SessionId = sessionId;
        Username = username;
        TaskId = taskId;
    }

    public override string Message => $"Estimation for user with name {Username} and task with id {TaskId} already exists in session with id {SessionId}";
}

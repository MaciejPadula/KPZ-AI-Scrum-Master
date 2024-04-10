namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Exceptions;

internal class EstimationAlreadyExistsException : Exception
{
    public string SessionId { get; private set; }
    public int UserId { get; private set; }
    public int TaskId { get; private set; }

    public EstimationAlreadyExistsException(string sessionId, int userId, int taskId)
    {
        SessionId = sessionId;
        UserId = userId;
        TaskId = taskId;
    }

    public override string Message => $"Estimation for user with id {UserId} and task with id {TaskId} already exists in session with id {SessionId}";
}

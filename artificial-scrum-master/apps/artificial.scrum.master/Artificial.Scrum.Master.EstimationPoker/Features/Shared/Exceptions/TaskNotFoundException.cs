namespace Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;

internal class TaskNotFoundException : Exception
{
    public int TaskId { get; private set; }

    public TaskNotFoundException(int taskId)
    {
        TaskId = taskId;
    }

    public override string Message => $"Task with id {TaskId} not found";
}

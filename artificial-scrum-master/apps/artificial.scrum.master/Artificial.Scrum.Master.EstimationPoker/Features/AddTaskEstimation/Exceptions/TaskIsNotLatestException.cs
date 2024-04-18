namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Exceptions;

internal class TaskIsNotLatestException : Exception
{
    public TaskIsNotLatestException(int taskId) : base($"Task {taskId} is not the latest")
    {
    }
}

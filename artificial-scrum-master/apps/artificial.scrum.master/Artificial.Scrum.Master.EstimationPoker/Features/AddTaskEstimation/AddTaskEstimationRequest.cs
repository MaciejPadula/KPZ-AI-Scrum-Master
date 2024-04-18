namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation;

internal record AddTaskEstimationRequest(
    string SessionId,
    string Username,
    int TaskId,
    decimal EstimationValue);

namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation;

internal record AddTaskEstimationRequest(
    string SessionId,
    int UserId,
    int TaskId,
    decimal EstimationValue);

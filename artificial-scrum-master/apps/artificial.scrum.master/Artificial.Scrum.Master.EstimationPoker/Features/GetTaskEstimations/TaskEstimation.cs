namespace Artificial.Scrum.Master.EstimationPoker.Features.GetTaskEstimations;

internal record TaskEstimation(
    int TaskId,
    int UserId,
    decimal Estimation);

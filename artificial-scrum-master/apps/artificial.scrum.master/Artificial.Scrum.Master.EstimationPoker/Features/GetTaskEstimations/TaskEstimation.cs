namespace Artificial.Scrum.Master.EstimationPoker.Features.GetTaskEstimations;

internal record TaskEstimation(
    int TaskId,
    string Username,
    decimal Estimation);

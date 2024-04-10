namespace Artificial.Scrum.Master.EstimationPoker.Features.GetTaskEstimations;
internal record GetTaskEstimationsResponse(
    List<TaskEstimation> Estimations,
    decimal AverageEstimation);

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetSuggestedEstimation;

internal record GetSuggestedEstimationRequest(
    int TaskId,
    List<decimal> Estimations);

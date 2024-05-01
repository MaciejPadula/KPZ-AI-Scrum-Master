namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

public readonly record struct GetSuggestedEstimationResult(
    decimal Value,
    string Reason);

namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

public readonly record struct SessionTaskEstimationEntity(
    int TaskId,
    string Username,
    decimal Value);

namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

public record SessionTaskEstimationEntity(
    int TaskId,
    int UserId,
    decimal Value);

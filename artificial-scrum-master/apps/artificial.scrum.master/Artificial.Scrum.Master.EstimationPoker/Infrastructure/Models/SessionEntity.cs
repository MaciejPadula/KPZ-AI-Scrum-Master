namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

public readonly record struct SessionEntity(
    string Id,
    string Name,
    int ProjectId,
    string OwnerId);

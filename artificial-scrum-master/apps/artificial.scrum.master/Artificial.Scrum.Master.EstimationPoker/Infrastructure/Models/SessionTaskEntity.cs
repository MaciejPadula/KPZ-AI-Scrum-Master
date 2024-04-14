namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

public readonly record struct SessionTaskEntity(
    string SessionId,
    string Title,
    string Description,
    DateTime CreatedAt)
{
    public int Id { get; init; }
}

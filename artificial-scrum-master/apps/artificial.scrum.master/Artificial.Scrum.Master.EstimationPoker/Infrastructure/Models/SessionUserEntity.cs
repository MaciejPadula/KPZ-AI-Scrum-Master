namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

public readonly record struct SessionUserEntity(
    string SessionId,
    string UserName)
{
    public int Id { get; init; }
}

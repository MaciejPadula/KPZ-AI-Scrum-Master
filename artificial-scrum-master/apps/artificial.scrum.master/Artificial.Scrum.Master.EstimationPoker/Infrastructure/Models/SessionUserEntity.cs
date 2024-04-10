namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

public record SessionUserEntity(
    string SessionId,
    string UserName)
{
    public int Id { get; init; }
}

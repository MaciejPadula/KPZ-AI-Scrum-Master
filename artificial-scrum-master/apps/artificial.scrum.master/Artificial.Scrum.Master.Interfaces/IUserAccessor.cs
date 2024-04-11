namespace Artificial.Scrum.Master.Interfaces;

public interface IUserAccessor
{
    string? UserId { get; }
    string? UserName { get; }
    string? PhotoUrl { get; }
}

namespace Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;

internal class UserNotFoundException : Exception
{
    public int UserId { get; private set; }

    public UserNotFoundException(int userId)
    {
        UserId = userId;
    }

    public override string Message => $"User with id {UserId} not found";
}

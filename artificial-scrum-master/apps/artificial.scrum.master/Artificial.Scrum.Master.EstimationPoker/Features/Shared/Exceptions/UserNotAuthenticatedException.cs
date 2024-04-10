namespace Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;

internal class UserNotAuthenticatedException : Exception
{
    public UserNotAuthenticatedException() : base("User is not authenticated.")
    {
    }
}

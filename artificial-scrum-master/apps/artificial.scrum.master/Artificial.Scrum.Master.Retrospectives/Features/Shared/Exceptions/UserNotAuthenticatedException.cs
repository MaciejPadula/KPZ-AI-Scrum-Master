using System.Runtime.Serialization;

namespace Artificial.Scrum.Master.Retrospectives.Features.Shared.Exceptions;

internal class UserNotAuthenticatedException : Exception
{
    public UserNotAuthenticatedException() : base("User is not authenticated.")
    {
    }
}

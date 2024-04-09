namespace Artificial.Scrum.Master.EstimationPoker.Features.Shared;

internal class UserNotAuthorizedException : Exception
{
    public string UserId { get; private set; }
    public string SessionId { get; private set; }

    public UserNotAuthorizedException(string userId, string sessionId)
    {
        UserId = userId;
        SessionId = sessionId;
    }

    public override string Message => $"User with id {UserId} is not authorized to use session {SessionId}";
}

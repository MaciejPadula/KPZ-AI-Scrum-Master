namespace Artificial.Scrum.Master.ScrumIntegration.Exceptions;

public class ProjectRequestForbidException : Exception
{
    public ProjectRequestForbidException(string? message) : base(message)
    {
    }
}

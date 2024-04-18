namespace Artificial.Scrum.Master.ScrumIntegration.Exceptions;

internal class ProjectRequestForbidException : Exception
{
    public ProjectRequestForbidException(string? message) : base(message)
    {
    }
}

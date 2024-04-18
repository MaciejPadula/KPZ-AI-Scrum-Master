namespace Artificial.Scrum.Master.ScrumIntegration.Exceptions;

internal class ProjectRequestFailedException : Exception
{
    public ProjectRequestFailedException(string message) : base(message)
    {
    }
}

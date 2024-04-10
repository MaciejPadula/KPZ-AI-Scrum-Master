namespace Artificial.Scrum.Master.ScrumIntegration.Exceptions
{
    public class RefreshTokenExpiredException : Exception
    {
        public RefreshTokenExpiredException(string message) : base(message)
        {
        }
    }
}

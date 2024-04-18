namespace Artificial.Scrum.Master.ScrumIntegration.Exceptions
{
    internal class RefreshTokenExpiredException : Exception
    {
        public RefreshTokenExpiredException(string message) : base(message)
        {
        }
    }
}

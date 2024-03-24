using System.Net;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Exceptions
{
    public class RequestFailedException : Exception
    {
        public RequestFailedException(HttpStatusCode responseStatusCode, string message) : base(message)
        {
        }
    }
}

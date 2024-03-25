using System.Net;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models
{
    public class WrapperHttpResponse
    {
        public string? Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}

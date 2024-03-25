using System.Net;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models
{
    public readonly record struct WrapperHttpResponse(
        string Response,
        HttpStatusCode StatusCode
    );
}

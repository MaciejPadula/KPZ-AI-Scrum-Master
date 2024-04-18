using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetSession;
internal static class GetSessionEndpoint
{
    public static void MapGetSessionEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/api/sessions/{sessionId}",
            async (HttpContext context, IGetSessionService service, string sessionId) =>
            {
                var response = await service.Handle(sessionId);
                await context.HandleResult(response);
            });
    }
}

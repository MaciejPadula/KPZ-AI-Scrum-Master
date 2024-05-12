using Artificial.Scrum.Master.Retrospectives.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.Retrospectives.Features.GetSprintSession;
internal static class GetSessionEndpoint
{
    public static void MapGetSessionEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/api/retrospectives/{sessionId}",
            async (string sessionId, HttpContext context, IGetSessionHandler handler) =>
            {
                var result = await handler.Handle(sessionId);
                await context.HandleResult(result);
            });
    }
};

using Artificial.Scrum.Master.Retrospectives.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.Retrospectives.Features.GetSuggestedIdeas;

internal static class GetSuggestedIdeasEndpoint
{
    public static void MapGetSuggestedIdeasEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/api/retrospectives/suggestions/{sessionId}",
            async (
                string sessionId,
                HttpContext context,
                IGetSuggestedIdeasHandler handler) =>
            {
                var result = await handler.Handle(sessionId);
                await context.HandleResult(result);
            });
    }
}

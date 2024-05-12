using Artificial.Scrum.Master.Retrospectives.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.Retrospectives.Features.GetSessionCards;
internal static class GetSessionCardsEndpoints
{
    public static void MapGetSessionCardsEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/api/retrospectives/cards/{sessionId}",
            async (string sessionId, HttpContext context, IGetSessionCardsHandler handler) =>
            {
                var result = await handler.Handle(sessionId);
                await context.HandleResult(result);
            });
    }
}

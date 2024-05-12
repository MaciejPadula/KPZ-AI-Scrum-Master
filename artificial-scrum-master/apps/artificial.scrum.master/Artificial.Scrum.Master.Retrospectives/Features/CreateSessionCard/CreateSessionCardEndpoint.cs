using Artificial.Scrum.Master.Retrospectives.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.Retrospectives.Features.CreateSessionCard;
internal static class CreateSessionCardEndpoint
{
    public static void MapCreateSessionCardEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost(
            "/api/retrospectives/cards",
            async (CreateSessionCardRequest request, HttpContext context, ICreateSessionCardHandler handler) =>
            {
                var result = await handler.Handle(request);
                await context.HandleResult(result);
            });
    }
}

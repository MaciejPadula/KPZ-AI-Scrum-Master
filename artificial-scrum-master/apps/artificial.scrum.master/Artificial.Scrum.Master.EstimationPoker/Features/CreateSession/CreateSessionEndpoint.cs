using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker.Features.CreateSession;

internal static class CreateSessionEndpoint
{
    public static void MapCreateSessionEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost(
            "/api/sessions",
            async (CreateSessionRequest request, HttpContext context, ICreateSessionService service) =>
            {
                var result = await service.Handle(request);
                await context.HandleResult(result);
            });
    }
}

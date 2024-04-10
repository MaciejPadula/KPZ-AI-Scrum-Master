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
            "/api/sessions/{projectId}",
            async (int projectId, HttpContext context, ICreateSessionService service) =>
            {
                var result = await service.Handle(projectId);
                await context.HandleResult(result);
            });
    }
}

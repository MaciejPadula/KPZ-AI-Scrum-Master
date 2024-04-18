using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetUserProjectSessions;

internal static class GetUserProjectSessionsEndpoint
{
    public static void MapGetUserProjectSessionsEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/api/sessions/project/{projectId}",
            async (HttpContext context, IGetUserProjectSessionsService service, int projectId) =>
            {
                var response = await service.Handle(projectId);
                await context.HandleResult(response);
            });
    }
}

using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetCurrentTask;

internal static class GetCurrentTaskEndpoint
{
    public static void MapGetCurrentTaskEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/api/sessions/tasks/{sessionId}",
            async (string sessionId, HttpContext context, IGetCurrentTaskService service) =>
            {
                var result = await service.Handle(sessionId);
                await context.HandleResult(result);
            });
    }
}

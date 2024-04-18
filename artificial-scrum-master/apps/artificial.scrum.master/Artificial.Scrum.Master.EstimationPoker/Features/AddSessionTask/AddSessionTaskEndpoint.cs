using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker.Features.AddSessionTask;

internal static class AddSessionTaskEndpoint
{
    public static void MapAddSessionTaskEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost(
            "/api/sessions/tasks",
            async (AddSessionTaskRequest request, HttpContext context, IAddSessionTaskService service) =>
            {
                var result = await service.Handle(request);
                await context.HandleResult(result);
            });
    }
}

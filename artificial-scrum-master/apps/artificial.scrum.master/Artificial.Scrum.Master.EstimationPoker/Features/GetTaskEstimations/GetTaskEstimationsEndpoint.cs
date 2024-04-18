using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetTaskEstimations;
internal static class GetTaskEstimationsEndpoint
{
    public static void MapGetTaskEstimationsEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/api/sessions/estimations/{taskId}",
            async (int taskId, HttpContext context, IGetTaskEstimationsService service) =>
            {
                var result = await service.Handle(taskId);
                await context.HandleResult(result);
            });
    }
}

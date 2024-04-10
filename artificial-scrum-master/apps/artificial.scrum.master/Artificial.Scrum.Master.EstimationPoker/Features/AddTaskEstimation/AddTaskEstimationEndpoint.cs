using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation;
internal static class AddTaskEstimationEndpoint
{
    public static void MapAddTaskEstimationEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost(
            "/api/sessions/estimations",
            async (AddTaskEstimationRequest request, HttpContext context, IAddTaskEstimationService service) =>
            {
                var result = await service.Handle(request);
                await context.HandleResult(result);
            });
    }
}

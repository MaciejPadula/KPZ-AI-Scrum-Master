using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetSuggestedEstimation;

internal static class GetSuggestedEstimationEndpoint
{
    public static void MapGetSuggestedEstimationEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost(
            "/api/sessions/estimations/suggestions",
            async (
                [FromBody] GetSuggestedEstimationRequest request,
                HttpContext context,
                IGetSuggestedEstimationService service) =>
            {
                var result = await service.Handle(request);
                await context.HandleResult(result);
            });
    }
}

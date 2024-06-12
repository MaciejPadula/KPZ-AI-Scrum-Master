using Artificial.Scrum.Master.Retrospectives.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.Retrospectives.Features.GetSuggestionForCard;

internal static class GetSuggestionForCardEndpoint
{
    public static void MapGetSuggestionForCardEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost(
            "/api/retrospectives/card-suggestions",
            async (
                [FromBody] GetSuggestionForCardRequest request,
                HttpContext context,
                IGetSuggestionForCardHandler handler) =>
            {
                var result = await handler.Handle(request);
                await context.HandleResult(result);
            });
    }
}

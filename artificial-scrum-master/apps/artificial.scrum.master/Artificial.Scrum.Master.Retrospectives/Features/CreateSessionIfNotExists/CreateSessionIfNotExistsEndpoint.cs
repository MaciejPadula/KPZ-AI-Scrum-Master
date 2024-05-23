using Artificial.Scrum.Master.Retrospectives.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.Retrospectives.Features.CreateSessionIfNotExists;

internal static class CreateSessionIfNotExistsEndpoint
{
    public static void MapCreateSessionIfNotExistsEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost(
            "/api/retrospectives/create-if-not-exists",
            async (CreateSessionIfNotExistsRequest request, HttpContext context, ICreateSessionIfNotExistsHandler handler) =>
            {
                var result = await handler.Handle(request);
                await context.HandleResult(result);
            });
    }
}

using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker.Features.AddSessionUser;

internal static class AddSessionUserEndpoint
{
    public static void MapAddSessionUserEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost(
            "/api/sessions/users",
            async (AddSessionUserRequest request, HttpContext context, IAddSessionUserService service) =>
            {
                var result = await service.Handle(request);
                await context.HandleResult(result);
            });
    }
}

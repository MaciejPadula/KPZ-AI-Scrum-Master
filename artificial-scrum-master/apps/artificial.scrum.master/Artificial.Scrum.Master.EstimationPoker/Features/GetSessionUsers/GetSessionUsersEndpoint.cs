using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetSessionUsers;

internal static class GetSessionUsersEndpoint
{
    public static void MapGetSessionUsersEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/api/sessions/users/{sessionId}",
            async (string sessionId, HttpContext context, IGetSessionUsersService service) =>
            {
                var result = await service.Handle(sessionId);
                await context.HandleResult(result);
            });
    }
}

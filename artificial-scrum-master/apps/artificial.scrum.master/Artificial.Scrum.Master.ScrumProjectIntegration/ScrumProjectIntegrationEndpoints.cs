using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumProjectIntegration.Features.Projects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.ScrumProjectIntegration;

public static class ScrumProjectIntegrationEndpoints
{
    public static void RegisterScrumProjectIntegrationEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/api/projects",
            async (HttpContext context, IGetUserProjectsService service, IUserAccessor userAccessor) =>
            {
                var userId = userAccessor.GetUserId();
                var result = await service.Handle(userId);

                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsJsonAsync(result);
            });
    }
}

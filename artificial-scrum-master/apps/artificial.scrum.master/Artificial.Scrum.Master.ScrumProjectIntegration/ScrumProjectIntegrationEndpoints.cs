using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumProjectIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumProjectIntegration.Features.Projects;
using Artificial.Scrum.Master.ScrumProjectIntegration.Features.Timeline;
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
                var userId = userAccessor.UserId;
                var result = await service.Handle(userId);

                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/timeline",
            async (HttpContext context, IGetProfileTimeLineService service, IUserAccessor userAccessor) =>
            {
                var userId = userAccessor.UserId;
                var result = await service.Handle(userId);

                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/timeline/{projectId}",
            async (HttpContext context, IGetProjectTimeLineService service, IUserAccessor userAccessor,
                string projectId) =>
            {
                var userId = userAccessor.UserId;
                var result = await service.Handle(userId, projectId);

                await context.Response.WriteAsJsonAsync(result);
            });
    }
}

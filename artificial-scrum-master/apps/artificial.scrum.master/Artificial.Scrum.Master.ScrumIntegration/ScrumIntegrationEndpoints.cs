using Artificial.Scrum.Master.ScrumIntegration.Features.Burndown;
using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Projects;
using Artificial.Scrum.Master.ScrumIntegration.Features.Sprints;
using Artificial.Scrum.Master.ScrumIntegration.Features.Tasks;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;
using Artificial.Scrum.Master.ScrumIntegration.Features.UserStories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.ScrumProjectIntegration;

public static class ScrumIntegrationEndpoints
{
    public static void RegisterScrumIntegrationEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/api/projects",
            async (HttpContext context, IGetUserProjectsService service) =>
            {
                var result = await service.Handle();
                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/timeline",
            async (HttpContext context, IGetProfileTimeLineService service) =>
            {
                var result = await service.Handle();
                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/timeline/{projectId}",
            async (HttpContext context, IGetProjectTimeLineService service, string projectId) =>
            {
                var result = await service.Handle(projectId);
                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/sprints/{projectId}",
            async (HttpContext context, IGetActiveSprintsService service, string projectId) =>
            {
                var result = await service.Handle(projectId);
                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/userStories",
            async (HttpContext context, IGetUserStoriesService service, [FromQuery] string projectId,
                [FromQuery] string sprintId) =>
            {
                var result = await service.Handle(projectId, sprintId);
                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/userStories/tasks",
            async (HttpContext context, IGetStoryTasksService service, [FromQuery] string? userStoryId,
                [FromQuery] string? sprintId) =>
            {
                if (string.IsNullOrEmpty(userStoryId) == string.IsNullOrEmpty(sprintId))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new
                        { Message = "Either userStoryId or sprintId is required" });
                    return;
                }

                var result = await service.Handle(userStoryId, sprintId);
                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet("/api/projects/sprints/{projectId}/stats",
            async (HttpContext context, IGetSprintStatsService service, [FromRoute] string projectId,
                [FromQuery] string sprintId) =>
            {
                var result = await service.Handle(projectId, sprintId);
                await context.Response.WriteAsJsonAsync(result);
            });
    }
}

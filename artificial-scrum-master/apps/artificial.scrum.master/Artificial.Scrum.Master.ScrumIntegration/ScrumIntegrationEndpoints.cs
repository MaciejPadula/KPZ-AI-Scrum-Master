using Artificial.Scrum.Master.Interfaces;
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
            async (HttpContext context, IGetUserProjectsService service, IUserAccessor userAccessor) =>
            {
                var userId = userAccessor.UserId;
                if (string.IsNullOrEmpty(userId))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { Message = "Unauthorized" });
                    return;
                }

                var result = await service.Handle(userId);

                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/timeline",
            async (HttpContext context, IGetProfileTimeLineService service, IUserAccessor userAccessor) =>
            {
                var userId = userAccessor.UserId;
                if (string.IsNullOrEmpty(userId))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { Message = "Unauthorized" });
                    return;
                }

                var result = await service.Handle(userId);

                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/timeline/{projectId}",
            async (HttpContext context, IGetProjectTimeLineService service, IUserAccessor userAccessor,
                string projectId) =>
            {
                var userId = userAccessor.UserId;
                if (string.IsNullOrEmpty(userId))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { Message = "Unauthorized" });
                    return;
                }

                var result = await service.Handle(userId, projectId);

                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/sprints/{projectId}",
            async (HttpContext context, IGetActiveSprintsService service, IUserAccessor userAccessor,
                string projectId) =>
            {
                var userId = userAccessor.UserId;
                if (string.IsNullOrEmpty(userId))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { Message = "Unauthorized" });
                    return;
                }

                var result = await service.Handle(userId, projectId);

                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/userStories",
            async (HttpContext context, IGetUserStoriesService service, IUserAccessor userAccessor,
                [FromQuery] string projectId, [FromQuery] string sprintId) =>
            {
                var userId = userAccessor.UserId;
                if (string.IsNullOrEmpty(userId))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { Message = "Unauthorized" });
                    return;
                }

                var result = await service.Handle(userId, projectId, sprintId);

                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet(
            "/api/projects/userStories/{userStoryId}/tasks",
            async (HttpContext context, IGetStoryTasksService service, IUserAccessor userAccessor,
                string userStoryId) =>
            {
                var userId = userAccessor.UserId;
                if (string.IsNullOrEmpty(userId))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { Message = "Unauthorized" });
                    return;
                }

                var result = await service.Handle(userId, userStoryId);

                await context.Response.WriteAsJsonAsync(result);
            });

        // todo: storyless tasks endpoint
    }
}

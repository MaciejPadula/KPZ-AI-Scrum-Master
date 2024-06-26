using Artificial.Scrum.Master.ScrumIntegration.Features.AddNewTask;
using Artificial.Scrum.Master.ScrumIntegration.Features.Burndown;
using Artificial.Scrum.Master.ScrumIntegration.Features.EditTaskDetails;
using Artificial.Scrum.Master.ScrumIntegration.Features.EditUserStoryDetails;
using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Projects;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TaskDetails;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.UserStoryDetails;
using Artificial.Scrum.Master.ScrumIntegration.Features.SprintOrder;
using Artificial.Scrum.Master.ScrumIntegration.Features.Sprints;
using Artificial.Scrum.Master.ScrumIntegration.Features.StoryTasks;
using Artificial.Scrum.Master.ScrumIntegration.Features.TaskDetails;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;
using Artificial.Scrum.Master.ScrumIntegration.Features.UserStories;
using Artificial.Scrum.Master.ScrumIntegration.Features.UserStoryDetails;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;
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

        routes.MapGet("/api/userStories/{storyId}",
            async (HttpContext context,
                IGetUserStoryDetailsService userStoryDetailsService,
                string storyId) =>
            {
                var result = await userStoryDetailsService.Handle(storyId);
                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapPatch("/api/userStories/{storyId}",
            async (HttpContext context, IPatchStoryService service, [FromRoute] string storyId,
                [FromBody] PatchStoryRequest request) =>
            {
                var result = await service.Handle(storyId, request);
                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet("/api/projects/sprints/{projectId}/stats",
            async (HttpContext context, IGetSprintStatsService service, [FromRoute] string projectId,
                [FromQuery] string sprintId) =>
            {
                var result = await service.Handle(projectId, sprintId);
                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapGet("/api/task/{taskId}",
            async (HttpContext context, IGetTaskDetailsService service, [FromRoute] string taskId) =>
            {
                var result = await service.Handle(taskId);
                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapPatch("/api/task/{taskId}",
            async (HttpContext context, IPatchTaskService service, [FromRoute] string taskId,
                [FromBody] PatchTaskRequest request) =>
            {
                var result = await service.Handle(taskId, request);
                await context.Response.WriteAsJsonAsync(result);
            });

        routes.MapPost("/api/task",
            async (HttpContext context, ICreateTaskService service,
                [FromBody] CreateTaskRequest request) =>
            {
                await service.Handle(request);
                context.Response.StatusCode = StatusCodes.Status201Created;
            });

        routes.MapPost("/api/projects/sprints/priority",
            async (HttpContext context, IUpdateSprintOrderService service,
                [FromBody] SprintOrderRequest request) =>
            {
                await service.Handle(request);
                context.Response.StatusCode = StatusCodes.Status200OK;
            });
    }
}

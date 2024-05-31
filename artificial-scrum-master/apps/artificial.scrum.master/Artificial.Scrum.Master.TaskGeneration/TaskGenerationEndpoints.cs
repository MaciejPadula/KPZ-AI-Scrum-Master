using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Artificial.Scrum.Master.TaskGeneration.Features.GenerateTasks;
using Microsoft.AspNetCore.Mvc;


namespace Artificial.Scrum.Master.TaskGeneration;
public static class TaskGenerationEndpoints
{
    public static void RegisterTaskGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/user-story/generate-tasks",
                       async (HttpContext context, IGetGenerateTaskService service,
                                      [FromBody] GenerateTasksRequest request) =>
                       {
                           var result = await service.GenerateTasks(request);
                           await context.Response.WriteAsJsonAsync(result);
                       }).RequireAuthorization("UserLoggedInPolicy");
    }
}

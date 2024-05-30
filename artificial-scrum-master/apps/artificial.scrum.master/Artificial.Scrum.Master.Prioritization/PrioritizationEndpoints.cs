using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Artificial.Scrum.Master.Prioritization.Features.GetStoryPrioritizationSuggestion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Artificial.Scrum.Master.Prioritization;

public static class PrioritizationEndpoints
{
    public static void RegisterPrioritySuggestionsEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/sprint/userStories/priority",
            async (HttpContext context, IGetStoryPrioritizationSuggestionService service,
                [FromQuery] string projectId, [FromQuery] string sprintId) =>
            {
                var result = await service.Handle(projectId, sprintId);
                await context.Response.WriteAsJsonAsync(result);
            }).RequireAuthorization("UserLoggedInPolicy");
    }
}

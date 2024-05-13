using Artificial.Scrum.Master.EditTextSuggestions.Features.GetEditStorySuggestion;
using Artificial.Scrum.Master.EditTextSuggestions.Features.GetEditTaskSuggestion;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Artificial.Scrum.Master.EditTextSuggestions;

public static class EditSuggestionEndpoints
{
    public static void RegisterEditSuggestionEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/task/suggestions",
            async (HttpContext context, IGetEditTaskSuggestionService service,
                [FromBody] GetEditTaskSuggestionRequest request) =>
            {
                var result = await service.Handle(request);
                await context.Response.WriteAsJsonAsync(result);
            }).RequireAuthorization("UserLoggedInPolicy");

        routes.MapPost("/api/userStory/suggestions",
            async (HttpContext context, IGetEditStorySuggestionService service,
                [FromBody] GetEditStorySuggestionRequest request) =>
            {
                var result = await service.Handle(request);
                await context.Response.WriteAsJsonAsync(result);
            }).RequireAuthorization("UserLoggedInPolicy");
    }
}

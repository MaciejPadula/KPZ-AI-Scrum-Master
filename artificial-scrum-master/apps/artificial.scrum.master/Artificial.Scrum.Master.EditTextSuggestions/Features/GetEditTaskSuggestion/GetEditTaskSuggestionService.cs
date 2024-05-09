using Artificial.Scrum.Master.EditTextSuggestions.Exceptions;
using Artificial.Scrum.Master.EditTextSuggestions.Infrastructure;

namespace Artificial.Scrum.Master.EditTextSuggestions.Features.GetEditTaskSuggestion;

internal interface IGetEditTaskSuggestionService
{
    Task<GetEditTaskSuggestionResponse> Handle(GetEditTaskSuggestionRequest request);
}

internal class GetEditTaskSuggestionService : IGetEditTaskSuggestionService
{
    private readonly ITaskSuggestionService _taskSuggestionService;

    public GetEditTaskSuggestionService(
        ITaskSuggestionService taskSuggestionService)
    {
        _taskSuggestionService = taskSuggestionService;
    }

    public async Task<GetEditTaskSuggestionResponse> Handle(GetEditTaskSuggestionRequest request)
    {
        var suggestion =
            await _taskSuggestionService.GetEditTaskSuggestion(
                request.UserStoryTitle ?? "This is a storyless task",
                request.TaskTitle,
                request.TaskDescription ?? string.Empty);

        if (!suggestion.HasValue)
        {
            throw new GenerateSuggestionFailException(
                $"Generating edit suggestion for task:{request.TaskTitle} failed");
        }

        return new GetEditTaskSuggestionResponse(request.TaskTitle, suggestion.Value.TaskDescriptionSuggestion);
    }
}

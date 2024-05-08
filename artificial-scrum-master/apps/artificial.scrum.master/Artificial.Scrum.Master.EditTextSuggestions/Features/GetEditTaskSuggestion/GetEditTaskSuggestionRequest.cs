namespace Artificial.Scrum.Master.EditTextSuggestions.Features.GetEditTaskSuggestion;

internal record GetEditTaskSuggestionRequest(
    string UserStoryTitle,
    string TaskTitle,
    string? TaskDescription
);

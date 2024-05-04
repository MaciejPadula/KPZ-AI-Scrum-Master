namespace Artificial.Scrum.Master.EditTextSuggestions.Features.GetEditTaskSuggestion;

internal record GetEditTaskSuggestionRequest(
    string TaskTitle,
    string? TaskDescription
);

namespace Artificial.Scrum.Master.EditTextSuggestions.Features.GetEditStorySuggestion;

internal record GetEditStorySuggestionRequest(
    string StoryTitle,
    string? StoryDescription
);

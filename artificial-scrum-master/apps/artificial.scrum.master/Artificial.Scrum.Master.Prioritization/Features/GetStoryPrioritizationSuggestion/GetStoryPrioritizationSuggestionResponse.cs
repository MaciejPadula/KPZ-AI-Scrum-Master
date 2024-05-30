namespace Artificial.Scrum.Master.Prioritization.Features.GetStoryPrioritizationSuggestion;

internal readonly record struct GetStoryPrioritizationSuggestionResponse(
    List<StoryPrioritySuggestion> Stories
);

internal readonly record struct StoryPrioritySuggestion(
    int? UserStoryId,
    string? UserStorySubject,
    int? UserStoryRef,
    int SprintId,
    string? SprintSlug,
    string? SprintName,
    int? SprintOrder
);

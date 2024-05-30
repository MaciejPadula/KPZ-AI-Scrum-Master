namespace Artificial.Scrum.Master.ScrumIntegration.SharedFeatures;

public readonly record struct GetStoriesWithTasksResponse(
    List<GetStoriesWithTaskResponseElement> Stories
);

public readonly record struct GetStoriesWithTaskResponseElement(
    int? UserStoryId,
    string? UserStorySubject,
    int? UserStoryRef,
    int SprintId,
    string? SprintSlug,
    string? SprintName,
    int? SprintOrder,
    IEnumerable<string> TaskNames
);

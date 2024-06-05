namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Handlers;

public readonly record struct GetStoriesWithTasksResponse(
    List<StoryWithTasksTitles> Stories
);

public readonly record struct StoryWithTasksTitles(
    int Id,
    string Subject,
    int Ref,
    int? SprintId,
    string? SprintSlug,
    string? SprintName,
    int? SprintOrder,
    List<string> TaskNames
);

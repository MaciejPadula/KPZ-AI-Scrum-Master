namespace Artificial.Scrum.Master.ScrumIntegration.Features.Sprints;

internal readonly record struct GetActiveSprintsResponse(
    List<ActiveSprint> Sprints
);

internal readonly record struct ActiveSprint(
    int ProjectId,
    string? ProjectName,
    string? ProjectSlug,
    int SprintId,
    string? SprintSlug,
    string? SprintName,
    string? EstimatedStart,
    string? EstimatedFinish,
    IEnumerable<ActiveSprintUserStory>? UserStories
);

internal readonly record struct ActiveSprintUserStory(
    int UserStoryId,
    string? UserStoryName,
    string? StatusName,
    bool IsClosed,
    int UserStoryRef,
    double TotalPoints
);

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Sprints;

internal readonly record struct GetActiveSprintsResponse(
    List<GetActiveSprintsResponseElement> Sprints
);

internal readonly record struct GetActiveSprintsResponseElement(
    int ProjectId,
    string? ProjectName,
    string? ProjectSlug,
    int SprintId,
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
    double TotalPoints
);

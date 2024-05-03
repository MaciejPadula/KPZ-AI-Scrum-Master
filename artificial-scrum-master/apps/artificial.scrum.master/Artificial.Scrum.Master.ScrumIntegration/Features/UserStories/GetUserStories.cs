namespace Artificial.Scrum.Master.ScrumIntegration.Features.UserStories;

internal readonly record struct GetUserStories(
    List<GetUserStoriesResponseElement> UserStories
);

internal readonly record struct GetUserStoriesResponseElement(
    int? UserStoryId,
    string? UserStorySubject,
    string? StatusName,
    string? StatusColor,
    int? UserStoryRef,
    bool IsClosed,
    string? AssignedToUsername,
    string? AssignedToFullNameDisplay,
    string? AssignedToPhoto,
    string? OwnerUsername,
    string? OwnerFullNameDisplay,
    string? OwnerPhoto,
    int SprintId,
    string? SprintSlug,
    string? SprintName,
    double? TotalPoints
);

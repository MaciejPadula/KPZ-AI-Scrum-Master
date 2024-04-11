namespace Artificial.Scrum.Master.ScrumIntegration.Features.UserStories;

internal readonly record struct GetUserStoriesResponse(
    List<GetUserStoriesResponseElement> UserStories
);

internal readonly record struct GetUserStoriesResponseElement(
    int UserStoryId,
    string? UserStorySubject,
    string? StatusName,
    int UserStoryRef,
    bool IsClosed,
    string? AssignedToUsername,
    string? AssignedToFullNameDisplay,
    string? AssignedToPhoto,
    string? OwnerUsername,
    string? OwnerFullNameDisplay,
    string? OwnerPhoto,
    int MileStoneId,
    string? MileStoneSlug,
    string? MileStoneName,
    double TotalPoints
);

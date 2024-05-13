namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.UserStoryDetails;

public readonly record struct GetUserStoryDetailsResponse(
    DateTime? Created,
    string? Description,
    string? DescriptionHtml,
    string? AssignedToName,
    string? AssignedToPhotoUrl,
    string? StatusColor,
    bool IsStatusClosed,
    string? StatusName,
    string? Title,
    int? Number,
    int Version
);

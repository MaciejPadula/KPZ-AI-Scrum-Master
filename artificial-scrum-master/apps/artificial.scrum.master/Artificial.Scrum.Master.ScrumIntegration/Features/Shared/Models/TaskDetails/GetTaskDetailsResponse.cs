namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TaskDetails;

internal readonly record struct GetTaskDetailsResponse(
    int TaskId,
    string? Subject,
    int TaskRef,
    List<List<string>>? Tags,
    string? DescriptionHtml,
    DateTime? CreatedDate,
    DateTime? FinishedDate,
    string? StatusName,
    string? StatusColor,
    string? OwnerUsername,
    string? OwnerFullName,
    string? OwnerPhoto,
    string? AssignedToUsername,
    string? AssignedToFullName,
    string? AssignedToPhoto,
    int UserStoryRef,
    string? UserStorySubject,
    int Version
);

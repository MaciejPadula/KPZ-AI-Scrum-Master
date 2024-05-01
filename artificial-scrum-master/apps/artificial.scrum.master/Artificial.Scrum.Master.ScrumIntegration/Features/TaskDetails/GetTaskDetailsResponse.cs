namespace Artificial.Scrum.Master.ScrumIntegration.Features.TaskDetails;

internal readonly record struct GetTaskDetailsResponse(
    int TaskId,
    string? Subject,
    int TaskRef,
    List<List<string>>? Tags,
    string? DescriptionHtml,
    DateTime? CreatedDate,
    DateTime? FinishedDate,
    string? MilestoneSlug,
    string? StatusName,
    string? StatusColor,
    string? OwnerUsername,
    string? OwnerFullName,
    string? OwnerPhoto,
    string? AssignedToUsername,
    string? AssignedToFullName,
    string? AssignedToPhoto,
    int UserStoryRef,
    string? UserStorySubject
);

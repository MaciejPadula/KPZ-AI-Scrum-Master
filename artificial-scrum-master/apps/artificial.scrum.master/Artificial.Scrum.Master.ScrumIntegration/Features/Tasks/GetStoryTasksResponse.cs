namespace Artificial.Scrum.Master.ScrumIntegration.Features.Tasks;

internal readonly record struct GetStoryTasksResponse(
    List<GetStoryTasksResponseElement> Tasks
);

internal readonly record struct GetStoryTasksResponseElement(
    int TaskId,
    string? Subject,
    int TaskRef,
    int TotalComments,
    List<string>? Tags,
    string? StatusName,
    string? StatusColor,
    string? AssignedToUsername,
    string? AssignedToFullName,
    string? AssignedToPhoto,
    bool IsClosed
);

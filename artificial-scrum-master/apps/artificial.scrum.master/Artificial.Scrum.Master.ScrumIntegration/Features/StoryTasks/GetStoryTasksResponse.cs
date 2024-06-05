namespace Artificial.Scrum.Master.ScrumIntegration.Features.StoryTasks;

internal readonly record struct GetStoryTasksResponse(
    List<UserStoryTask> Tasks
);

internal readonly record struct UserStoryTask(
    int TaskId,
    string? Subject,
    int TaskRef,
    int TotalComments,
    List<List<string>>? Tags,
    string? StatusName,
    string? StatusColor,
    string? AssignedToUsername,
    string? AssignedToFullName,
    string? AssignedToPhoto,
    bool IsClosed
);

namespace Artificial.Scrum.Master.TaskGeneration.Features.GenerateTasks;
public readonly record struct GenerateTasksRequest
(
    string UserStoryTitle,
    string UserStoryDescription
);

namespace Artificial.Scrum.Master.TaskGeneration.Features.GenerateTasks;
public readonly record struct GenerateTasksRequest
{
    public string UserStoryTitle { get; init; }
    public string UserStoryDescription { get; init; }

}

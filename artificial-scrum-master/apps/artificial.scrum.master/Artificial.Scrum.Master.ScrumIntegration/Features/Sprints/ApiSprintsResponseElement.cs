using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Sprints;

internal class Sprint
{
    public int Project { get; set; }
    [JsonPropertyName("project_extra_info")] public ProjectExtraInfo? ProjectExtraInfo { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
    [JsonPropertyName("estimated_start")] public string? EstimatedStart { get; set; }
    [JsonPropertyName("estimated_finish")] public string? EstimatedFinish { get; set; }
    [JsonPropertyName("user_stories")] public List<UserStory>? UserStories { get; set; }
}

internal class UserStory
{
    [JsonPropertyName("status_extra_info")] public StatusExtraInfo? StatusExtraInfo { get; set; }
    public int Id { get; set; }
    [JsonPropertyName("is_closed")] public bool IsClosed { get; set; }
    public string? Subject { get; set; }
    [JsonPropertyName("total_points")] public double TotalPoints { get; set; }
}

internal class ProjectExtraInfo
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
}

internal class StatusExtraInfo
{
    public string? Name { get; set; }
}

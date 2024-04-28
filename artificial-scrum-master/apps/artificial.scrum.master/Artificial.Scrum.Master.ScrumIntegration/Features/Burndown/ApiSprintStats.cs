using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Burndown;

public class SprintStats
{
    public string? Name { get; set; }
    [JsonPropertyName("estimated_start")] public string? EstimatedStart { get; set; }
    [JsonPropertyName("estimated_finish")] public string? EstimatedFinish { get; set; }
    [JsonPropertyName("total_points")] public TotalPoints? TotalPoints { get; set; }
    [JsonPropertyName("completed_points")] public List<double>? CompletedPoints { get; set; }
    [JsonPropertyName("total_userstories")] public int TotalUserStories { get; set; }
    [JsonPropertyName("completed_userstories")] public int CompletedUserStories { get; set; }
    [JsonPropertyName("total_tasks")] public int TotalTasks { get; set; }
    [JsonPropertyName("completed_tasks")] public int CompletedTasks { get; set; }
    public List<DayData>? Days { get; set; }
}

public class DayData
{
    public string? Day { get; set; }
    public int Name { get; set; }
    [JsonPropertyName("open_points")] public double OpenPoints { get; set; }
    [JsonPropertyName("optimal_points")] public double OptimalPoints { get; set; }
}

public class TotalPoints
{
    [JsonPropertyName("9262872")] public double RoleId9262872 { get; set; }
    [JsonPropertyName("9262871")] public double RoleId9262871 { get; set; }
    [JsonPropertyName("9262870")] public double RoleId9262870 { get; set; }
    [JsonPropertyName("9262869")] public double RoleId9262869 { get; set; }
}

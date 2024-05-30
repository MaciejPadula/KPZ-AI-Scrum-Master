using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.UserStory;

public class UserStory
{
    public int? Id { get; set; }
    [JsonPropertyName("ref")] public int? Ref { get; set; }
    public string? Subject { get; set; }
    [JsonPropertyName("status_extra_info")] public StatusExtraInfo? StatusExtraInfo { get; set; }
    [JsonPropertyName("assigned_to_extra_info")] public UserInfo? AssignedToExtraInfo { get; set; }
    [JsonPropertyName("owner_extra_info")] public UserInfo? OwnerExtraInfo { get; set; }
    public int Milestone { get; set; }
    [JsonPropertyName("milestone_slug")] public string? MilestoneSlug { get; set; }
    [JsonPropertyName("milestone_name")] public string? MilestoneName { get; set; }
    [JsonPropertyName("total_points")] public double? TotalPoints { get; set; }
    [JsonPropertyName("sprint_order")] public int? SprintOrder { get; set; }
}

public class UserInfo
{
    public string? Username { get; set; }
    [JsonPropertyName("full_name_display")] public string? FullNameDisplay { get; set; }
    public string? Photo { get; set; }
}

public class StatusExtraInfo
{
    public string? Name { get; set; }
    [JsonPropertyName("is_closed")] public bool IsClosed { get; set; }
    public string? Color { get; set; }
}

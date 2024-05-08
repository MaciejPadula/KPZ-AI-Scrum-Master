using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TaskDetails;

public class TaskSpecifics
{
    public List<List<string>>? Tags { get; set; }
    [JsonPropertyName("status_extra_info")] public StatusExtraInfo? StatusExtraInfo { get; set; }
    [JsonPropertyName("assigned_to_extra_info")] public UserInfo? AssignedToExtraInfo { get; set; }
    [JsonPropertyName("owner_extra_info")] public UserInfo? OwnerExtraInfo { get; set; }
    public int Id { get; set; }
    [JsonPropertyName("ref")] public int Ref { get; set; }
    [JsonPropertyName("created_date")] public DateTime? CreatedDate { get; set; }
    [JsonPropertyName("finished_date")] public DateTime? FinishedDate { get; set; }
    public string? Subject { get; set; }
    [JsonPropertyName("user_story_extra_info")] public UserStoryExtraInfo? UserStoryExtraInfo { get; set; }
    [JsonPropertyName("description_html")] public string? DescriptionHtml { get; set; }
    public int Version { get; set; }
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
    public string? Color { get; set; }
}

public class UserStoryExtraInfo
{
    [JsonPropertyName("ref")] public int Ref { get; set; }
    public string? Subject { get; set; }
}

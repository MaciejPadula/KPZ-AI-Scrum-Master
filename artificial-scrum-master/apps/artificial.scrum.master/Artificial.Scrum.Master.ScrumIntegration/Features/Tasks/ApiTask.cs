using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Tasks;

public class StoryTask
{
    public int Id { get; set; }
    public string? Subject { get; set; }
    [JsonPropertyName("ref")] public int Ref { get; set; }
    [JsonPropertyName("total_comments")] public int TotalComments { get; set; }
    public List<List<string>>? Tags { get; set; }
    [JsonPropertyName("status_extra_info")] public StatusExtraInfo? StatusExtraInfo { get; set; }
    [JsonPropertyName("assigned_to_extra_info")] public UserInfo? AssignedToExtraInfo { get; set; }
    [JsonPropertyName("is_closed")] public bool IsClosed { get; set; }
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

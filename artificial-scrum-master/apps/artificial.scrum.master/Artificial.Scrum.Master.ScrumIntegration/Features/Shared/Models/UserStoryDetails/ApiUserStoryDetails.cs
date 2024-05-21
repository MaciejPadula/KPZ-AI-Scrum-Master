using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.UserStoryDetails;

internal class UserStorySpecifics
{
    [JsonPropertyName("assigned_to_extra_info")] public AssignedToExtraInfo? AssignedToInfo { get; set; }
    [JsonPropertyName("status_extra_info")] public StatusExtraInfo? StatusInfo { get; set; }
    [JsonPropertyName("created_date")] public DateTime? Created { get; set; }
    [JsonPropertyName("description_html")] public string? DescriptionHtml { get; set; }
    public string? Description { get; set; }
    [JsonPropertyName("subject")] public string? Title { get; set; }
    [JsonPropertyName("ref")] public int? Number { get; set; }
    public int Version { get; set; }
}

internal class AssignedToExtraInfo
{
    [JsonPropertyName("full_name_display")] public string? Name { get; set; }
    [JsonPropertyName("photo")] public string? PhotoUrl { get; set; }
}

internal class StatusExtraInfo
{
    [JsonPropertyName("color")] public string? Color { get; set; }
    [JsonPropertyName("is_closed")] public bool Closed { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.UserStoryDetails;
internal class UserStoryDetailsResponse
{
    [JsonPropertyName("assigned_to_extra_info")] public AssignedToExtraInfo? AssignedToInfo { get; set; }
    [JsonPropertyName("status_extra_info")] public StatusExtraInfo? StatusInfo { get; set; }
    [JsonPropertyName("created_date")] public DateTime? Created { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("subject")] public string? Title { get; set; }

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

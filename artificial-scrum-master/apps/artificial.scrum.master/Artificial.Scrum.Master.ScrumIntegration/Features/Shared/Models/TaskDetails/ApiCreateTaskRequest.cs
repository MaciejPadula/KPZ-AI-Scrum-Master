using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TaskDetails;

internal class ApiCreateTaskRequest
{
    public string? Description { get; set; }
    public int? Project { get; set; }
    public string? Subject { get; set; }
    [JsonPropertyName("taskboard_order")] public int? TaskboardOrder { get; set; }
    [JsonPropertyName("us_order")] public int? UsOrder { get; set; }
    [JsonPropertyName("user_story")] public int? UserStory { get; set; }
}

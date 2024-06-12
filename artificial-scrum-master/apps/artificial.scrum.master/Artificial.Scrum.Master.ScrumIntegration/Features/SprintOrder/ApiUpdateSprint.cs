using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.SprintOrder;

public class ApiUpdateSprint
{
    [JsonPropertyName("bulk_stories")] public required List<BulkStory> Stories { get; set; }
    [JsonPropertyName("milestone_id")] public int SprintId { get; set; }
    [JsonPropertyName("project_id")] public int ProjectId { get; set; }
}

public class BulkStory
{
    public int Order { get; set; }
    [JsonPropertyName("us_id")] public int StoryId { get; set; }
}

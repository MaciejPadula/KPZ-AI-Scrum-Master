using System.Text.Json.Serialization;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared;
using Task = Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Task;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

internal class ProfileTimeLineElementRoot
{
    public required Data Data { get; set; }
    public int Id { get; set; }
    [JsonPropertyName("event_type")] public string? EventType { get; set; }
    public int Project { get; set; }
    public DateTime Created { get; set; }
}

internal class Data
{
    public Task? Task { get; set; }
    public required User User { get; set; }
    public string? Comment { get; set; }
    public required Shared.Project Project { get; set; }
    [JsonPropertyName("values_diff")] public required ValuesDiff ValuesDiff { get; set; }
    public Milestone? Milestone { get; set; }
    public Userstory? Userstory { get; set; }
}

internal class Milestone
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

using System.Text.Json.Serialization;

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
    public required Project Project { get; set; }
    [JsonPropertyName("values_diff")] public required ValuesDiff ValuesDiff { get; set; }
    public Milestone? Milestone { get; set; }
    public Userstory? Userstory { get; set; }
}

internal class ValuesDiff
{
    public Attachments? Attachments { get; set; }
    [JsonPropertyName("assigned_to")] public List<string>? AssignedTo { get; set; }
    public List<List<string>>? Tags { get; set; }
    public List<string>? Subject { get; set; }
    public List<string>? Status { get; set; }
}

internal class Attachments
{
    [JsonPropertyName("new")] public List<New>? New { get; set; }
    public List<object>? Changed { get; set; }
    public List<object>? Deleted { get; set; }
}

internal class Milestone
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

internal class New
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    [JsonPropertyName("thumb_url")] public string? ThumbUrl { get; set; }
}

internal class Project
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

internal class Task
{
    public int Id { get; set; }
    public string? Subject { get; set; }
    public Userstory? Userstory { get; set; }
}

internal class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Photo { get; set; }
    public string? Username { get; set; }
}

internal class Userstory
{
    public int Id { get; set; }
    public string? Subject { get; set; }
}

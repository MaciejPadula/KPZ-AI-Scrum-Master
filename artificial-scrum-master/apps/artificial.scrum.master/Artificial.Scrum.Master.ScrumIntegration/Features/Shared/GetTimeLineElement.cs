namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

internal abstract record GetTimeLineElement
{
    public int Id { get; set; }
    public string? EventType { get; set; }
    public DateTime Created { get; set; }
    public int ProjectId { get; set; }
    public int TaskId { get; set; }
    public string? TaskSubject { get; set; }
    public int TaskUserStoryId { get; set; }
    public string? TaskUserStorySubject { get; set; }
    public int UserId { get; set; }
    public required string UserName { get; set; }
    public string? UserPhoto { get; set; }
    public string? UserUsername { get; set; }
    public required string ProjectName { get; set; }
    public required IEnumerable<KeyValuePair<string, string>> ValuesDiff { get; set; }
}

using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.ResponseEnums;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

internal abstract record GetTimeLineEvent
{
    public int Id { get; set; }
    public required ScrumObjectType ScrumObjectType { get; set; }
    public required ScrumObjectState ScrumObjectState { get; set; }
    public DateTime Created { get; set; }
    public int ProjectId { get; set; }
    public int TaskId { get; set; }
    public string? TaskSubject { get; set; }
    public int TaskUserStoryId { get; set; }
    public string? TaskUserStorySubject { get; set; }
    public int UserId { get; set; }
    public required string UserName { get; set; }
    public string? UserPhoto { get; set; }
    public string? UserNick { get; set; }
    public required string ProjectName { get; set; }
    public required List<KeyValuePair<string, string>> ValuesDiff { get; set; }
}

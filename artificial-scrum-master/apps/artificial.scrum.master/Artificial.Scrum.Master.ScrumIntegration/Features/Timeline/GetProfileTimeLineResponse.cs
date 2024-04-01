using Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

internal readonly record struct GetProfileTimeLineResponse(
    IEnumerable<GetProfileTimeLineResponseEvent> TimeLineEvents
);

internal record GetProfileTimeLineResponseEvent : GetTimeLineEvent
{
    public int MileStoneId { get; set; }
    public string? MileStoneName { get; set; }
    public int UserStoryId { get; set; }
    public string? UserStorySubject { get; set; }
}

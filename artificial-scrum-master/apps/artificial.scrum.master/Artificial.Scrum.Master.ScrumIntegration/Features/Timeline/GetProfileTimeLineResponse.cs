using Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

internal readonly record struct GetProfileTimeLineResponse(
    IEnumerable<GetProfileTimeLineResponseElement> TimeLineEvents
);

internal record GetProfileTimeLineResponseElement : GetTimeLineEvent
{
    public int MileStoneId { get; set; }
    public string? MileStoneName { get; set; }
    public int UserStoryId { get; set; }
    public string? UserStorySubject { get; set; }
}

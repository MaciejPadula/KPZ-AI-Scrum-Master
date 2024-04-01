using Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

internal readonly record struct GetProfileTimeLineResponse(
    IEnumerable<GetProfileTimeLineResponseElement> Elements
);

internal record GetProfileTimeLineResponseElement : GetTimeLineElement
{
    public int MileStoneId { get; set; }
    public required string MileStoneName { get; set; }
    public int UserStoryId { get; set; }
    public string? UserStorySubject { get; set; }
}

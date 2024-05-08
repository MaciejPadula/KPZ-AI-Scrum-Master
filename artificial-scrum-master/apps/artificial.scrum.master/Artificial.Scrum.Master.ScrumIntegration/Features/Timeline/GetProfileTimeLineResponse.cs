using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TimeLine;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

internal readonly record struct GetProfileTimeLineResponse(
    IEnumerable<TimeLineEvent> TimeLineEvents
);

using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

internal readonly record struct GetProfileTimeLineResponse(
    IEnumerable<GetTimeLineEvent> TimeLineEvents
);

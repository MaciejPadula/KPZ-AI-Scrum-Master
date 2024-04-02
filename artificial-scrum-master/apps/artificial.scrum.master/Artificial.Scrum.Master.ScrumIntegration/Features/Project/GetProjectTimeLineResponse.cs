using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Project;

internal readonly record struct GetProjectTimeLineResponse(
    IEnumerable<GetTimeLineEvent> TimeLineEvents
);

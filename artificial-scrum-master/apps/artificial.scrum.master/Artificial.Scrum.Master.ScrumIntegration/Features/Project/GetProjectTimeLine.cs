using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TimeLine;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Project;

internal readonly record struct GetProjectTimeLine(
    IEnumerable<TimeLineEvent> TimeLineEvents
);

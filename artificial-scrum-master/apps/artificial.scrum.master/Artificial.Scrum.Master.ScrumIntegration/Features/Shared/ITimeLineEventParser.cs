using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

internal interface ITimeLineEventParser
{
    GetProfileTimeLineResponse ParseProfileTimeLineElement(IEnumerable<TimeLineEventRoot> elements);
    GetProjectTimeLineResponse ParseProjectTimeLineElement(IEnumerable<TimeLineEventRoot> elements);
}

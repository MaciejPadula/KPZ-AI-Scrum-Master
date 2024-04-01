using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

internal interface ITimeLineEventParser
{
    GetProfileTimeLineResponse ParseProfileTimeLineElement(IEnumerable<ProfileTimeLineElementRoot> elements);
    GetProjectTimeLineResponse ParseProjectTimeLineElement(IEnumerable<ProjectTimeLineElementRoot> elements);
}

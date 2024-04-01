using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

internal interface ITimeLineEventParser
{
    GetProfileTimeLineResponse ParseProfileTimeLineElement(List<ProfileTimeLineElementRoot> elements);
    GetProjectTimeLineResponse ParseProjectTimeLineElement(List<ProjectTimeLineElementRoot> elements);
}

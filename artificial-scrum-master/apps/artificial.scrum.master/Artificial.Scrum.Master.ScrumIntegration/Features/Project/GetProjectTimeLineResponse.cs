using Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Project;

internal readonly record struct GetProjectTimeLineResponse(
    IEnumerable<GetProjectTimeLineResponseElement> Elements
);

internal record GetProjectTimeLineResponseElement : GetTimeLineElement
{
}

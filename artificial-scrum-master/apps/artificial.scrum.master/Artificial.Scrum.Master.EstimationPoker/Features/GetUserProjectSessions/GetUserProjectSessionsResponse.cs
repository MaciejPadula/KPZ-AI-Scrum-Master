using Artificial.Scrum.Master.EstimationPoker.Features.Shared;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetUserProjectSessions;

internal record GetUserProjectSessionsResponse(
    List<Session> Sessions);

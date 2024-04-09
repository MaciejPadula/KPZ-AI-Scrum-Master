using Artificial.Scrum.Master.EstimationPoker.Features.AddSessionTask;
using Artificial.Scrum.Master.EstimationPoker.Features.AddSessionUser;
using Artificial.Scrum.Master.EstimationPoker.Features.GetSessionUsers;
using Artificial.Scrum.Master.EstimationPoker.Features.GetUserProjectSessions;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker;

public static class EstimationPokerEndpoints
{
    public static void RegisterEstimationPokerEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGetUserProjectSessionsEndpoint();

        routes.MapAddSessionUserEndpoint();
        routes.MapGetSessionUsersEndpoint();

        routes.MapAddSessionTaskEndpoint();
    }
}

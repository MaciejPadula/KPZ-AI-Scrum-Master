using Artificial.Scrum.Master.EstimationPoker.Features.AddSessionTask;
using Artificial.Scrum.Master.EstimationPoker.Features.AddSessionUser;
using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation;
using Artificial.Scrum.Master.EstimationPoker.Features.CreateSession;
using Artificial.Scrum.Master.EstimationPoker.Features.GetCurrentTask;
using Artificial.Scrum.Master.EstimationPoker.Features.GetSessionUsers;
using Artificial.Scrum.Master.EstimationPoker.Features.GetTaskEstimations;
using Artificial.Scrum.Master.EstimationPoker.Features.GetUserProjectSessions;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.EstimationPoker;

public static class EstimationPokerEndpoints
{
    public static void RegisterEstimationPokerEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapCreateSessionEndpoint();
        routes.MapGetUserProjectSessionsEndpoint();

        routes.MapAddSessionUserEndpoint();
        routes.MapGetSessionUsersEndpoint();

        routes.MapAddSessionTaskEndpoint();
        routes.MapGetCurrentTaskEndpoint();

        routes.MapAddTaskEstimationEndpoint();
        routes.MapGetTaskEstimationsEndpoint();
    }
}

using Artificial.Scrum.Master.EstimationPoker.Features.AddSessionUser;
using Artificial.Scrum.Master.EstimationPoker.Features.GetUserProjectSessions;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.EstimationPoker;

public static class EstimationPokerModule
{
    public static void AddEstimationPokerModule(this IServiceCollection services)
    {
        services.AddTransient<IGetUserProjectSessionsService, GetUserProjectSessionsService>();
        services.AddTransient<IAddSessionUserService, AddSessionUserService>();
    }
}

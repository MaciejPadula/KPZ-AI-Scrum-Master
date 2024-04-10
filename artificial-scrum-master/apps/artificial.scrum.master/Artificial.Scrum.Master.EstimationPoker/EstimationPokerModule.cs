using Artificial.Scrum.Master.EstimationPoker.Features.AddSessionTask;
using Artificial.Scrum.Master.EstimationPoker.Features.AddSessionUser;
using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation;
using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator;
using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator.Estimation;
using Artificial.Scrum.Master.EstimationPoker.Features.GetSessionUsers;
using Artificial.Scrum.Master.EstimationPoker.Features.GetUserProjectSessions;
using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.EstimationPoker;

public static class EstimationPokerModule
{
    public static void AddEstimationPokerModule(this IServiceCollection services)
    {
        services.AddTransient<IGetUserProjectSessionsService, GetUserProjectSessionsService>();
        services.AddTransient<IAddSessionUserService, AddSessionUserService>();
        services.AddTransient<IGetSessionUsersService, GetSessionUsersService>();
        services.AddTransient<IAddSessionTaskService, AddSessionTaskService>();
        services.AddTransient<IAddTaskEstimationService, AddTaskEstimationService>();

        services.AddTransient<IRequestValidator, RequestValidator>();
        services.AddTransient<IEstimationValidator, MinEstimationValidator>();
        services.AddTransient<IEstimationValidator, ModuloEstimationValidator>();
    }
}

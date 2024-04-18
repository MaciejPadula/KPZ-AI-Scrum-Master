using Artificial.Scrum.Master.EstimationPoker.Features.AddSessionTask;
using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation;
using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator;
using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator.Estimation;
using Artificial.Scrum.Master.EstimationPoker.Features.CreateSession;
using Artificial.Scrum.Master.EstimationPoker.Features.GetCurrentTask;
using Artificial.Scrum.Master.EstimationPoker.Features.GetSession;
using Artificial.Scrum.Master.EstimationPoker.Features.GetTaskEstimations;
using Artificial.Scrum.Master.EstimationPoker.Features.GetUserProjectSessions;
using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.EstimationPoker;

public static class EstimationPokerModule
{
    public static void AddEstimationPokerModule(this IServiceCollection services)
    {
        services.AddTransient<IGetUserProjectSessionsService, GetUserProjectSessionsService>();
        services.AddTransient<IAddSessionTaskService, AddSessionTaskService>();
        services.AddTransient<IAddTaskEstimationService, AddTaskEstimationService>();
        services.AddTransient<ICreateSessionService, CreateSessionService>();
        services.AddTransient<IGetCurrentTaskService, GetCurrentTaskService>();
        services.AddTransient<IGetSessionService, GetSessionService>();
        services.AddTransient<IGetTaskEstimationsService, GetTaskEstimationsService>();
        services.AddTransient<ISessionKeyGenerator, SessionKeyGenerator>();

        services.AddTransient<IRequestValidator, RequestValidator>();
        services.AddTransient<IEstimationValidator, MinEstimationValidator>();
        services.AddTransient<IEstimationValidator, ModuloEstimationValidator>();
    }
}

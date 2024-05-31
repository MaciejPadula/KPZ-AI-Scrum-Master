using Artificial.Scrum.Master.TaskGeneration.Features.GenerateTasks;
using Artificial.Scrum.Master.TaskGeneration.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.TaskGeneration;
public static class TaskGenerationModule
{
    public static IServiceCollection AddTaskGenerationModule(this IServiceCollection services)
    {
        services.AddTransient<IGetGenerateTaskService, GetGenerateTaskService>();
        services.AddTransient<TaskGenerationMiddleware>();
        return services;
    }

    public static IApplicationBuilder UseTaskGenerationModule(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TaskGenerationMiddleware>();
    }
}

using Artificial.Scrum.Master.ScrumIntegration;
using Artificial.Scrum.Master.ScrumIntegration.Features.AddNewTask;
using Artificial.Scrum.Master.ScrumIntegration.Features.Burndown;
using Artificial.Scrum.Master.ScrumIntegration.Features.EditTaskDetails;
using Artificial.Scrum.Master.ScrumIntegration.Features.EditUserStoryDetails;
using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Projects;
using Artificial.Scrum.Master.ScrumIntegration.Features.Sprints;
using Artificial.Scrum.Master.ScrumIntegration.Features.TaskDetails;
using Artificial.Scrum.Master.ScrumIntegration.Features.Tasks;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;
using Artificial.Scrum.Master.ScrumIntegration.Features.UserStories;
using Artificial.Scrum.Master.ScrumIntegration.Features.UserStoryDetails;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Middleware;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.BurnDown;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.Sprints;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.Tasks;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.TimelineEvents;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.UserStories;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.ScrumProjectIntegration;

public static class ScrumIntegrationModule
{
    public static IServiceCollection AddScrumIntegrationModule(
        this IServiceCollection services,
        IConfigurationSection scrumManagementServiceSettings)
    {
        var agileServiceBaseUri = new Uri(scrumManagementServiceSettings["BaseUrl"] ?? throw new
            InvalidOperationException("Base Url for agile service integration required"));

        services.AddHttpClient(
            Consts.TaigaClient,
            c => c.BaseAddress = agileServiceBaseUri);
        services.AddTransient<IProjectHttpClientWrapper, ProjectHttpClientWrapper>();
        services.AddHttpClient<ITokenRefresher, TokenRefresher>(c =>
            c.BaseAddress = agileServiceBaseUri);

        services.AddTransient<ITimeLineEventMapper, TimeLineEventMapper>();
        services.AddTransient<ITimeLineEventObjectsParser, TimeLineEventObjectsParser>();
        services.AddTransient<ISprintsResponseMapper, SprintsResponseMapper>();
        services.AddTransient<IUserStoriesMapper, UserStoriesMapper>();
        services.AddTransient<ITasksResponseMapper, TasksResponseMapper>();
        services.AddTransient<IRolePointsMapper, RolePointsMapper>();
        services.AddTransient<ISprintStatsResponseMapper, SprintStatsResponseMapper>();
        services.AddTransient<ITaskDetailsResponseMapper, TaskDetailsResponseMapper>();

        services.AddTransient<IJwtDecoder, JwtDecoder>();
        services.AddTransient<ITokenValidator, TokenValidator>();
        services.AddTransient<IAccessTokenProvider, TokenProvider>();

        services.AddTransient<IGetUserProjectsService, GetUserProjectsService>();
        services.AddTransient<IGetProfileTimeLineService, GetProfileTimeLineService>();
        services.AddTransient<IGetProjectTimeLineService, GetProjectTimeLineService>();
        services.AddTransient<IGetActiveSprintsService, GetActiveSprintsService>();
        services.AddTransient<IGetUserStoriesService, GetUserStoriesService>();
        services.AddTransient<IGetStoryTasksService, GetStoryTasksService>();
        services.AddTransient<IGetUserStoryDetailsService, GetUserStoryDetailsService>();
        services.AddTransient<IPatchStoryService, PatchStoryService>();
        services.AddTransient<IGetSprintStatsService, GetSprintStatsService>();
        services.AddTransient<IGetTaskDetailsService, GetTaskDetailsService>();
        services.AddTransient<IPatchTaskService, PatchTaskService>();
        services.AddTransient<ICreateTaskService, CreateTaskService>();

        services.AddTransient<ScrumIntegrationMiddleware>();

        return services;
    }

    public static IApplicationBuilder UseScrumProjectIntegration(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ScrumIntegrationMiddleware>();
    }
}

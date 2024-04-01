using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Projects;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Middleware;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
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
        services.AddHttpClient<IProjectHttpClientWrapper, ProjectHttpClientWrapper>(c =>
        {
            c.BaseAddress = new Uri(scrumManagementServiceSettings["BaseUrl"] ?? throw new
                InvalidOperationException("Base Url for agile service integration required"));
        });
        services.AddTransient<ITimeLineEventParser, TimeLineEventParser>();
        services.AddTransient<IJwtDecoder, JwtDecoder>();
        services.AddTransient<ITokenValidator, TokenValidator>();
        services.AddTransient<IAccessTokenProvider, TokenProvider>();
        services.AddTransient<IGetUserProjectsService, GetUserProjectsService>();
        services.AddTransient<IGetProfileTimeLineService, GetProfileTimeLineService>();
        services.AddTransient<IGetProjectTimeLineService, GetProjectTimeLineService>();

        services.AddTransient<ScrumIntegrationMiddleware>();

        return services;
    }

    public static IApplicationBuilder UseScrumProjectIntegration(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ScrumIntegrationMiddleware>();
    }
}

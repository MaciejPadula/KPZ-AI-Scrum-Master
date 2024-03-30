using Artificial.Scrum.Master.ScrumProjectIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumProjectIntegration.Features.Projects;
using Artificial.Scrum.Master.ScrumProjectIntegration.Features.Timeline;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Middleware;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumProjectIntegration.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.ScrumProjectIntegration;

public static class ScrumProjectIntegrationModule
{
    public static IServiceCollection AddScrumProjectIntegration(
        this IServiceCollection services,
        IConfigurationSection scrumManagementServiceSettings)
    {
        services.AddHttpClient<IProjectHttpClientWrapper, ProjectHttpClientWrapper>(c =>
        {
            c.BaseAddress = new Uri(scrumManagementServiceSettings["BaseUrl"] ?? throw new
                InvalidOperationException("Base Url for agile service integration required"));
        });
        services.AddTransient<IJwtDecoder, JwtDecoder>();
        services.AddTransient<ITokenValidator, TokenValidator>();
        services.AddTransient<IGetUserProjectsService, GetUserProjectsService>();
        services.AddTransient<IGetProfileTimeLineService, GetProfileTimeLineService>();
        services.AddTransient<IGetProjectTimeLineService, GetProjectTimeLineService>();

        services.AddTransient<ScrumProjectIntegrationMiddleware>();

        return services;
    }

    public static IApplicationBuilder UseScrumProjectIntegration(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ScrumProjectIntegrationMiddleware>();
    }
}

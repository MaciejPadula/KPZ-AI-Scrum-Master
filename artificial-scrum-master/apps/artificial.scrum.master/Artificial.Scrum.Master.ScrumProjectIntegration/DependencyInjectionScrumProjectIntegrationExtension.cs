using Artificial.Scrum.Master.ScrumProjectIntegration.Features.Projects;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumProjectIntegration.Settings;
using Artificial.Scrum.Master.ScrumProjectIntegration.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.ScrumProjectIntegration
{
    public static class DependencyInjectionScrumProjectIntegrationExtension
    {
        public static IServiceCollection AddScrumProjectIntegration(
            this IServiceCollection services,
            IConfigurationSection scrumManagementServiceSettings)
        {
            var settings = new ScrumManagementServiceSettings();
            scrumManagementServiceSettings.Bind(settings);
            services.AddSingleton(settings);

            services.AddSingleton<IJwtDecoder, JwtDecoder>();

            services.AddHttpClient<IProjectHttpClientWrapper, ProjectHttpClientWrapper>(c =>
            {
                c.BaseAddress = new Uri(settings.BaseUrl);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            // TODO: mocked Token Repository
            services.AddSingleton<IUserTokensRepository, MockedUserTokensRepository>();
            //

            services.AddScoped<IGetUserProjectsService, GetUserProjectsService>();

            return services;
        }
    }
}

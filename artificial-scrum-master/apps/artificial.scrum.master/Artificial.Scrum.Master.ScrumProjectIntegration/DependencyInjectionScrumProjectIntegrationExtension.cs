using Artificial.Scrum.Master.ScrumProjectIntegration.Settings;
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

            return services;
        }
    }
}

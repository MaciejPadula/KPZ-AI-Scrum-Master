using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Infrastructure.ExternalServices;
using Artificial.Scrum.Master.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Extensions;

namespace Artificial.Scrum.Master.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string sqlConnectionString)
    {
        services.AddTransient<IDbConnectionFactory>(_ => new SqlDbConnectionFactory(sqlConnectionString));
        services.AddTransient<IUserSettingsRepository, SqlUserSettingsRepository>();
        services.AddTransient<IUserTokensRepository, SqlUserSettingsRepository>();
        services.AddTransient<ISessionRepository, SqlSessionRepository>();
        services.AddTransient<ISessionTaskRepository, SqlSessionTaskRepository>();
        services.AddTransient(_ => TimeProvider.System);
        services.AddHttpContextAccessor();

        services.AddOpenAIService();
        services.AddTransient<IPokerSuggestionService, OpenAIPokerSuggestionService>();

        services.AddTransient<IUserAccessor, JwtUserAccessor>();

        services.AddSingleton<IActiveUserRepository, InMemorySessionUserRepository>();
        return services;
    }
}

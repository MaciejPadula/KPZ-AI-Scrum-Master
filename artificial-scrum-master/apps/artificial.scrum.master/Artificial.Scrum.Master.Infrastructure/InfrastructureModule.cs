using Artificial.Scrum.Master.EditTextSuggestions.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Infrastructure.Authorization.Handlers;
using Artificial.Scrum.Master.Infrastructure.Authorization.Requirements;
using Artificial.Scrum.Master.Infrastructure.ExternalServices;
using Artificial.Scrum.Master.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.TokenRefresher.Interfaces;
using Artificial.Scrum.Master.TaskGeneration.Features.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Extensions;

namespace Artificial.Scrum.Master.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string sqlConnectionString, InfrastructureType infrastructureType = InfrastructureType.AspNetCore)
    {
        services.AddTransient<IDbConnectionFactory>(_ => new SqlDbConnectionFactory(sqlConnectionString));
        services.AddTransient<IUserSettingsRepository, SqlUserSettingsRepository>();
        services.AddTransient<IUserTokensRepository, SqlUserSettingsRepository>();
        services.AddTransient<ITokenRepository, SqlUserSettingsRepository>();
        services.AddTransient<ISessionRepository, SqlSessionRepository>();
        services.AddTransient<ISessionTaskRepository, SqlSessionTaskRepository>();
        services.AddTransient<IRetroSessionRepository, SqlRetroSessionRepository>();
        services.AddTransient(_ => TimeProvider.System);

        services.AddOpenAIService();
        services.AddTransient<IPokerSuggestionService, OpenAIPokerSuggestionService>();
        services.AddTransient<IStorySuggestionService, OpenAIStorySuggestionService>();
        services.AddTransient<ITaskSuggestionService, OpenAITaskSuggestionService>();
        services.AddTransient<IRetroSuggestionService, OpenAIRetroSuggestionService>();
        services.AddTransient<ITaskGenerationService, OpenAITaskGenerationService>();

        services.AddSingleton<IActiveUserRepository, InMemorySessionUserRepository>();

        if (infrastructureType == InfrastructureType.AspNetCore)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IUserAccessor, JwtUserAccessor>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserLoggedInPolicy",
                    policy => policy.Requirements.Add(new LoggedInRequirement(["UserId", "UserName"])));
            });
            services.AddScoped<IAuthorizationHandler, UserLoggedInRequirementHandler>();
        }

        return services;
    }
}

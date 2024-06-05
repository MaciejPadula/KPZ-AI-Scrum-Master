using Microsoft.Extensions.DependencyInjection;
using Artificial.Scrum.Master.Prioritization.Features.GetStoryPrioritizationSuggestion;
using Artificial.Scrum.Master.Prioritization.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Artificial.Scrum.Master.Prioritization;

public static class PrioritizationModule
{
    public static IServiceCollection AddPrioritySuggestionsModule(this IServiceCollection services)
    {
        services.AddTransient<IGetStoryPrioritizationSuggestionService, GetStoryPrioritizationSuggestionService>();
        services.AddTransient<IStoryPrioritizationMapper, StoryPrioritizationMapper>();
        services.AddTransient<PrioritizationMiddleware>();

        return services;
    }

    public static IApplicationBuilder UsePrioritySuggestionsModule(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<PrioritizationMiddleware>();
    }
}

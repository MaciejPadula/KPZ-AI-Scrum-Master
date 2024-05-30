using Microsoft.Extensions.DependencyInjection;
using Artificial.Scrum.Master.Prioritization.Features.GetStoryPrioritizationSuggestion;

namespace Artificial.Scrum.Master.Prioritization;

public static class PrioritizationModule
{
    public static IServiceCollection AddPrioritySuggestionsModule(this IServiceCollection services)
    {
        services.AddTransient<IGetStoryPrioritizationSuggestionService, GetStoryPrioritizationSuggestionService>();

        return services;
    }
}

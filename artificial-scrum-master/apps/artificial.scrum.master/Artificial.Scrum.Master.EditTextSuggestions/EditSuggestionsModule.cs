using Artificial.Scrum.Master.EditTextSuggestions.Features.GetEditStorySuggestion;
using Artificial.Scrum.Master.EditTextSuggestions.Features.GetEditTaskSuggestion;
using Artificial.Scrum.Master.EditTextSuggestions.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.EditTextSuggestions;

public static class EditSuggestionsModule
{
    public static IServiceCollection AddEditSuggestionsModule(this IServiceCollection services)
    {
        services.AddTransient<IGetEditTaskSuggestionService, GetEditTaskSuggestionService>();
        services.AddTransient<IGetEditStorySuggestionService, GetEditStorySuggestionService>();

        services.AddTransient<EditSuggestionMiddleware>();

        return services;
    }

    public static IApplicationBuilder UseEditSuggestionsModule(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<EditSuggestionMiddleware>();
    }
}

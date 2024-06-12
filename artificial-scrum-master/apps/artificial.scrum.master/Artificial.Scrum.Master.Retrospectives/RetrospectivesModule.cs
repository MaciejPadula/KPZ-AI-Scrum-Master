using Artificial.Scrum.Master.Retrospectives.Features.CreateSessionCard;
using Artificial.Scrum.Master.Retrospectives.Features.CreateSessionIfNotExists;
using Artificial.Scrum.Master.Retrospectives.Features.GetSessionCards;
using Artificial.Scrum.Master.Retrospectives.Features.GetSprintSession;
using Artificial.Scrum.Master.Retrospectives.Features.GetSuggestedIdeas;
using Artificial.Scrum.Master.Retrospectives.Features.GetSuggestionForCard;
using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.Retrospectives;

public static class RetrospectivesModule
{
    public static void AddRetrospectives(this IServiceCollection services)
    {
        services.AddTransient<ICreateSessionIfNotExistsHandler, CreateSessionIfNotExistsHandler>();
        services.AddTransient<IGetSessionHandler, GetSessionHandler>();
        services.AddTransient<ICreateSessionCardHandler, CreateSessionCardHandler>();
        services.AddTransient<IGetSessionCardsHandler, GetSessionCardsHandler>();
        services.AddTransient<IGetSuggestedIdeasHandler, GetSuggestedIdeasHandler>();
        services.AddTransient<IGetSuggestionForCardHandler, GetSuggestionForCardHandler>();
    }
}

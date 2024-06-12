using Artificial.Scrum.Master.Retrospectives.Features.CreateSessionCard;
using Artificial.Scrum.Master.Retrospectives.Features.CreateSessionIfNotExists;
using Artificial.Scrum.Master.Retrospectives.Features.GetSessionCards;
using Artificial.Scrum.Master.Retrospectives.Features.GetSprintSession;
using Artificial.Scrum.Master.Retrospectives.Features.GetSuggestedIdeas;
using Artificial.Scrum.Master.Retrospectives.Features.GetSuggestionForCard;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.Retrospectives;

public static class RetrospectivesEndpoints
{
    public static void RegisterRetrospectivesEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapCreateSessionIfNotExistsEndpoint();
        routes.MapGetSessionEndpoint();

        routes.MapCreateSessionCardEndpoint();
        routes.MapGetSessionCardsEndpoints();

        routes.MapGetSuggestedIdeasEndpoint();
        routes.MapGetSuggestionForCardEndpoint();
    }
}

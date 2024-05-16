using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;

namespace Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;

public interface IRetroSuggestionService
{
    Task<GetSuggestedIdeasResult?> GetSuggestedIdeas(IEnumerable<SessionCard> cards);
}

using Artificial.Scrum.Master.Prioritization.Infrastructure.Models;

namespace Artificial.Scrum.Master.Prioritization.Infrastructure;

public interface IStoryPrioritizationSuggestionService
{
    Task<GetStoryPrioritizationSuggestionResult?> GetStoryPrioritizationSuggestion(
        StoriesPrioritySuggestionRequest suggestionRequest, bool generateAgain);
}

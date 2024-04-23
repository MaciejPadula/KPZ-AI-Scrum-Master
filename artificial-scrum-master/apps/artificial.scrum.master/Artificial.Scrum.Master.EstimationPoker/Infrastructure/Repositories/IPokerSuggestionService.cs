using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;

namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

public interface IPokerSuggestionService
{
    Task<GetSuggestedEstimationResult?> GetSuggestedEstimation(string taskTitle, string taskDescription, List<decimal> estimations);
}

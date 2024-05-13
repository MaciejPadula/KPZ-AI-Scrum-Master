using Artificial.Scrum.Master.EditTextSuggestions.Infrastructure.Models;

namespace Artificial.Scrum.Master.EditTextSuggestions.Infrastructure;

public interface IStorySuggestionService
{
    Task<GetEditUserStorySuggestionResult?> GetEditUserStorySuggestion(
        string userStoryTitle,
        string userStoryDescription);
}

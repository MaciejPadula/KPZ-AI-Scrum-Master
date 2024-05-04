using Artificial.Scrum.Master.EditTextSuggestions.Infrastructure.Models;

namespace Artificial.Scrum.Master.EditTextSuggestions.Infrastructure.BusinessLogic;

public interface ITaskSuggestionService
{
    Task<GetEditTaskSuggestionResult?> GetEditTaskSuggestion(string taskTitle, string taskDescription);
}

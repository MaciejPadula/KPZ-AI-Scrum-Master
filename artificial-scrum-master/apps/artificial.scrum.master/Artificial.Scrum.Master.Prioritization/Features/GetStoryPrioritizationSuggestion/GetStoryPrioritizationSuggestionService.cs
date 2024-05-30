namespace Artificial.Scrum.Master.Prioritization.Features.GetStoryPrioritizationSuggestion;

internal interface IGetStoryPrioritizationSuggestionService
{
    Task<GetStoryPrioritizationSuggestionResponse> Handle(string projectId, string sprintId);
}

internal class GetStoryPrioritizationSuggestionService : IGetStoryPrioritizationSuggestionService
{
    public Task<GetStoryPrioritizationSuggestionResponse> Handle(string projectId, string sprintId)
    {
        throw new NotImplementedException();
    }
}

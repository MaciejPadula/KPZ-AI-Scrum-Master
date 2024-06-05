using Artificial.Scrum.Master.Prioritization.Exceptions;
using Artificial.Scrum.Master.Prioritization.Infrastructure;
using Artificial.Scrum.Master.Prioritization.Infrastructure.Models;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Handlers;

namespace Artificial.Scrum.Master.Prioritization.Features.GetStoryPrioritizationSuggestion;

internal interface IGetStoryPrioritizationSuggestionService
{
    Task<GetStoryPrioritizationSuggestionResponse> Handle(string projectId, string sprintId, bool generateAgain);
}

internal class GetStoryPrioritizationSuggestionService : IGetStoryPrioritizationSuggestionService
{
    private readonly IStoryPrioritizationSuggestionService _prioritizationSuggestionService;
    private readonly IGetStoriesWithTasksHandler _getStoriesWithTasksHandler;
    private readonly IStoryPrioritizationMapper _storyPrioritizationMapper;

    public GetStoryPrioritizationSuggestionService(
        IStoryPrioritizationSuggestionService prioritizationSuggestionService,
        IGetStoriesWithTasksHandler getStoriesWithTasksHandler,
        IStoryPrioritizationMapper storyPrioritizationMapper)
    {
        _prioritizationSuggestionService = prioritizationSuggestionService;
        _getStoriesWithTasksHandler = getStoriesWithTasksHandler;
        _storyPrioritizationMapper = storyPrioritizationMapper;
    }

    public async Task<GetStoryPrioritizationSuggestionResponse> Handle(
        string projectId,
        string sprintId,
        bool generateAgain)
    {
        var storiesWithTasks = await _getStoriesWithTasksHandler.Handle(projectId, sprintId);
        var stories = _storyPrioritizationMapper.MapStoriesWithTasks(storiesWithTasks.Stories);

        var prioritizationSuggestion = await _prioritizationSuggestionService.GetStoryPrioritizationSuggestion(
            new StoriesPrioritySuggestionRequest(stories), generateAgain);

        if (!prioritizationSuggestion.HasValue)
        {
            throw new GeneratePrioritySuggestionFailException(
                $"Generating stories priority suggestion for sprint:{sprintId} failed");
        }

        return _storyPrioritizationMapper.MapPrioritySuggestionResponse(storiesWithTasks.Stories,
            prioritizationSuggestion.Value.UserStories);
    }
}

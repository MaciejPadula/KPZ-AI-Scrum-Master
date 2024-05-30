using Artificial.Scrum.Master.Prioritization.Exceptions;
using Artificial.Scrum.Master.Prioritization.Infrastructure;
using Artificial.Scrum.Master.Prioritization.Infrastructure.Models;
using Artificial.Scrum.Master.ScrumIntegration.SharedFeatures;

namespace Artificial.Scrum.Master.Prioritization.Features.GetStoryPrioritizationSuggestion;

internal interface IGetStoryPrioritizationSuggestionService
{
    Task<GetStoryPrioritizationSuggestionResponse> Handle(string projectId, string sprintId, bool generateAgain);
}

internal class GetStoryPrioritizationSuggestionService : IGetStoryPrioritizationSuggestionService
{
    private readonly IStoryPrioritizationSuggestionService _prioritizationSuggestionService;
    private readonly IGetStoriesWithTasksService _getStoriesWithTasksService;

    public GetStoryPrioritizationSuggestionService(
        IStoryPrioritizationSuggestionService prioritizationSuggestionService,
        IGetStoriesWithTasksService getStoriesWithTasksService)
    {
        _prioritizationSuggestionService = prioritizationSuggestionService;
        _getStoriesWithTasksService = getStoriesWithTasksService;
    }

    public async Task<GetStoryPrioritizationSuggestionResponse> Handle(
        string projectId,
        string sprintId,
        bool generateAgain)
    {
        var storiesWithTasks = await _getStoriesWithTasksService.Handle(projectId, sprintId);

        var stories = new List<StoryWithTasks>();
        foreach (var story in storiesWithTasks.Stories)
        {
            if (!story.UserStoryId.HasValue || string.IsNullOrEmpty(story.UserStorySubject))
            {
                continue;
            }

            stories.Add(new StoryWithTasks(
                story.UserStoryId.Value,
                story.UserStorySubject!,
                story.TaskNames.ToList()
            ));
        }

        var prioritizationSuggestion = await _prioritizationSuggestionService.GetStoryPrioritizationSuggestion(
            new StoriesPrioritySuggestionRequest(stories), generateAgain);

        if (!prioritizationSuggestion.HasValue)
        {
            throw new GeneratePrioritySuggestionFailException(
                $"Generating stories priority suggestion for sprint:{sprintId} failed");
        }
        // todo: middleware dodac!!!

        return ReorderModelsById(storiesWithTasks.Stories, prioritizationSuggestion.Value.UserStories);
    }

    private GetStoryPrioritizationSuggestionResponse ReorderModelsById(
        List<GetStoriesWithTaskResponseElement> original,
        List<UserStory> suggestion)
    {
        Dictionary<int, GetStoriesWithTaskResponseElement> modelDict = new();
        foreach (var story in original)
        {
            if (!story.UserStoryId.HasValue || string.IsNullOrEmpty(story.UserStorySubject))
            {
                continue;
            }

            modelDict.Add(story.UserStoryId.Value, story);
        }

        var orderedModels = suggestion
            .Select(us => modelDict[us.UserStoryId])
            .ToList();

        return new GetStoryPrioritizationSuggestionResponse(
            Stories: orderedModels.Select(s => new StoryPrioritySuggestion(
                s.UserStoryId,
                s.UserStorySubject,
                s.UserStoryRef,
                s.SprintId,
                s.SprintSlug,
                s.SprintName,
                s.SprintOrder
            )).ToList()
        );
    }
}

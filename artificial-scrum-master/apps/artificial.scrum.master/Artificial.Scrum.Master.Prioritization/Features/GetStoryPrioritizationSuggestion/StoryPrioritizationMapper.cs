using Artificial.Scrum.Master.Prioritization.Infrastructure.Models;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.GetStoriesWithTasks;

namespace Artificial.Scrum.Master.Prioritization.Features.GetStoryPrioritizationSuggestion;

internal interface IStoryPrioritizationMapper
{
    List<StoryWithTasks> MapStoriesWithTasks(List<GetStoriesWithTaskResponseElement> responseElements);

    GetStoryPrioritizationSuggestionResponse MapPrioritySuggestionResponse(
        List<GetStoriesWithTaskResponseElement> original,
        List<UserStory> suggestion);
}

internal class StoryPrioritizationMapper : IStoryPrioritizationMapper
{
    public List<StoryWithTasks> MapStoriesWithTasks(List<GetStoriesWithTaskResponseElement> responseElements)
    {
        var stories = responseElements
            .Where(story => story.UserStoryId.HasValue && !string.IsNullOrEmpty(story.UserStorySubject))
            .Select(story => new StoryWithTasks(
                story.UserStoryId!.Value,
                story.UserStorySubject!,
                story.TaskNames.ToList()))
            .ToList();

        return stories;
    }

    public GetStoryPrioritizationSuggestionResponse MapPrioritySuggestionResponse(
        List<GetStoriesWithTaskResponseElement> original,
        List<UserStory> suggestion)
    {
        var modelDict = original
            .Where(story => story.UserStoryId.HasValue && !string.IsNullOrEmpty(story.UserStorySubject))
            .ToDictionary(story => story.UserStoryId!.Value);

        var orderedModels = suggestion
            .Select(us => modelDict[us.UserStoryId])
            .ToList();

        var storyPrioritySuggestions = orderedModels
            .Select(s => new StoryPrioritySuggestion(
                s.UserStoryId,
                s.UserStorySubject,
                s.UserStoryRef,
                s.SprintId,
                s.SprintSlug,
                s.SprintName,
                s.SprintOrder))
            .ToList();

        return new GetStoryPrioritizationSuggestionResponse(Stories: storyPrioritySuggestions);
    }
}

using Artificial.Scrum.Master.Prioritization.Infrastructure.Models;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Handlers;

namespace Artificial.Scrum.Master.Prioritization.Features.GetStoryPrioritizationSuggestion;

internal interface IStoryPrioritizationMapper
{
    List<StoryWithTasks> MapStoriesWithTasks(List<StoryWithTasksTitles> responseElements);

    GetStoryPrioritizationSuggestionResponse MapPrioritySuggestionResponse(
        List<StoryWithTasksTitles> original,
        List<UserStory> suggestion);
}

internal class StoryPrioritizationMapper : IStoryPrioritizationMapper
{
    public List<StoryWithTasks> MapStoriesWithTasks(List<StoryWithTasksTitles> responseElements)
    {
        var stories = responseElements
            .Where(story => !string.IsNullOrEmpty(story.Subject))
            .Select(story => new StoryWithTasks(
                story.Id,
                story.Subject,
                story.TaskNames.ToList())
            ).ToList();

        return stories;
    }

    public GetStoryPrioritizationSuggestionResponse MapPrioritySuggestionResponse(
        List<StoryWithTasksTitles> original,
        List<UserStory> suggestion)
    {
        var modelDict = original
            .Where(story => !string.IsNullOrEmpty(story.Subject))
            .ToDictionary(story => story.Id);

        var orderedModels = suggestion
            .Select(us => modelDict[us.UserStoryId])
            .ToList();

        var storyPrioritySuggestions = orderedModels
            .Select(s => new StoryPrioritySuggestion(
                s.Id,
                s.Subject,
                s.Ref,
                s.SprintId,
                s.SprintSlug,
                s.SprintName,
                s.SprintOrder))
            .ToList();

        return new GetStoryPrioritizationSuggestionResponse(storyPrioritySuggestions);
    }
}

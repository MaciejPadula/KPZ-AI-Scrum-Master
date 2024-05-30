namespace Artificial.Scrum.Master.Prioritization.Infrastructure.Models;

public readonly record struct GetStoryPrioritizationSuggestionResult(
    List<UserStory> UserStories
);

public readonly record struct UserStory(
    string Title,
    int UserStoryId
);

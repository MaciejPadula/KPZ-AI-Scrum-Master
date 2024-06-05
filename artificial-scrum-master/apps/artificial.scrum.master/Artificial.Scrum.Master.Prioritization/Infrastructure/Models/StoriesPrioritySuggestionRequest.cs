namespace Artificial.Scrum.Master.Prioritization.Infrastructure.Models;

public readonly record struct StoriesPrioritySuggestionRequest(
    List<StoryWithTasks> Stories
);

public readonly record struct StoryWithTasks(
    int UserStoryId,
    string Title,
    List<string> Tasks
);

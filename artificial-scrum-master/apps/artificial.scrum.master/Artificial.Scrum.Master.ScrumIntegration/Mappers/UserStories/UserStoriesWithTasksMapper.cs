using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Handlers;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.Task;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.UserStory;

namespace Artificial.Scrum.Master.ScrumIntegration.Mappers.UserStories;

internal interface IUserStoriesWithTasksMapper
{
    GetStoriesWithTasksResponse MapUserStoriesWithTasksResponse(
        List<UserStory> userStoriesWithTasks,
        List<StoryTask> tasks);
}

internal class UserStoriesWithTasksMapper : IUserStoriesWithTasksMapper
{
    public GetStoriesWithTasksResponse MapUserStoriesWithTasksResponse(
        List<UserStory> userStoriesWithTasks,
        List<StoryTask> tasks)
    {
        var taskNames = tasks
            .Where(t =>
                t.UserStoryId is not null &&
                !string.IsNullOrEmpty(t.Subject)
            )
            .GroupBy(t => t.UserStoryId!.Value)
            .ToDictionary(
                t => t.Key,
                t => t
                    .Select(task => task.Subject ?? string.Empty)
                    .ToList()
            );

        var mappedUserStories = userStoriesWithTasks
            .OrderBy(us => us.SprintOrder)
            .Select(us => new StoryWithTasksTitles
            {
                Id = us.Id,
                Subject = us.Subject ?? string.Empty,
                Ref = us.Ref,
                SprintId = us.Milestone,
                SprintSlug = us.MilestoneSlug,
                SprintName = us.MilestoneName,
                SprintOrder = us.SprintOrder,
                TaskNames = taskNames[us.Id],
            }).ToList();

        return new GetStoriesWithTasksResponse(mappedUserStories);
    }
}

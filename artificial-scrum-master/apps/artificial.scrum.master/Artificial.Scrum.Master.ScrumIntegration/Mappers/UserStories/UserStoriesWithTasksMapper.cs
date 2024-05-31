using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.GetStoriesWithTasks;
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
        var mappedUserStories = userStoriesWithTasks.OrderBy(us => us.SprintOrder)
            .Select(us =>
            {
                var taskNames = tasks
                    .Where(t =>
                        !string.IsNullOrEmpty(t.Subject) &&
                        t.UserStoryId is not null &&
                        t.UserStoryId == us.Id)
                    .Select(t => t.Subject ?? string.Empty)
                    .ToList();

                return new GetStoriesWithTaskResponseElement
                {
                    UserStoryId = us.Id,
                    UserStorySubject = us.Subject,
                    UserStoryRef = us.Ref,
                    SprintId = us.Milestone,
                    SprintSlug = us.MilestoneSlug,
                    SprintName = us.MilestoneName,
                    SprintOrder = us.SprintOrder,
                    TaskNames = taskNames
                };
            }).ToList();

        return new GetStoriesWithTasksResponse(Stories: mappedUserStories);
    }
}

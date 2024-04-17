using Artificial.Scrum.Master.ScrumIntegration.Features.Tasks;

namespace Artificial.Scrum.Master.ScrumIntegration.Mappers.Tasks;

internal interface ITasksResponseMapper
{
    GetStoryTasksResponse MapUserStoriesResponse(List<StoryTask> userStories);
}

internal class TasksResponseMapper : ITasksResponseMapper
{
    public GetStoryTasksResponse MapUserStoriesResponse(List<StoryTask> userStories)
    {
        var mappedTasks = userStories.Select(x => new UserStoryTask
        {
            TaskId = x.Id,
            Subject = x.Subject,
            TaskRef = x.Ref,
            TotalComments = x.TotalComments,
            Tags = x.Tags,
            StatusName = x.StatusExtraInfo?.Name,
            StatusColor = x.StatusExtraInfo?.Color,
            AssignedToUsername = x.AssignedToExtraInfo?.Username,
            AssignedToFullName = x.AssignedToExtraInfo?.FullNameDisplay,
            AssignedToPhoto = x.AssignedToExtraInfo?.Photo,
            IsClosed = x.IsClosed
        }).ToList();

        return new GetStoryTasksResponse(Tasks: mappedTasks);
    }
}

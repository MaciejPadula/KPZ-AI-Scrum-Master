using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.Task;
using Artificial.Scrum.Master.ScrumIntegration.Features.StoryTasks;

namespace Artificial.Scrum.Master.ScrumIntegration.Mappers.Tasks;

internal interface ITasksResponseMapper
{
    GetStoryTasksResponse MapTasksResponse(List<StoryTask> tasks);
}

internal class TasksResponseMapper : ITasksResponseMapper
{
    public GetStoryTasksResponse MapTasksResponse(List<StoryTask> tasks)
    {
        var mappedTasks = tasks.Select(x => new UserStoryTask
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

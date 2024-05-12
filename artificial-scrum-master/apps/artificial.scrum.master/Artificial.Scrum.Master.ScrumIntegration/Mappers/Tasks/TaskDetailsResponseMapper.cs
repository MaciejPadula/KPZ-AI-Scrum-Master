using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TaskDetails;

namespace Artificial.Scrum.Master.ScrumIntegration.Mappers.Tasks;

internal interface ITaskDetailsResponseMapper
{
    GetTaskDetailsResponse MapTaskDetailsResponse(TaskSpecifics taskSpecifics);
}

internal class TaskDetailsResponseMapper : ITaskDetailsResponseMapper
{
    public GetTaskDetailsResponse MapTaskDetailsResponse(TaskSpecifics taskSpecifics)
    {
        return new GetTaskDetailsResponse(
            TaskId: taskSpecifics.Id,
            Subject: taskSpecifics.Subject,
            TaskRef: taskSpecifics.Ref,
            Tags: taskSpecifics.Tags,
            Description: taskSpecifics.Description,
            DescriptionHtml: taskSpecifics.DescriptionHtml,
            CreatedDate: taskSpecifics.CreatedDate,
            FinishedDate: taskSpecifics.FinishedDate,
            StatusName: taskSpecifics.StatusExtraInfo?.Name,
            StatusColor: taskSpecifics.StatusExtraInfo?.Color,
            OwnerUsername: taskSpecifics.OwnerExtraInfo?.Username,
            OwnerFullName: taskSpecifics.OwnerExtraInfo?.FullNameDisplay,
            OwnerPhoto: taskSpecifics.OwnerExtraInfo?.Photo,
            AssignedToUsername: taskSpecifics.AssignedToExtraInfo?.Username,
            AssignedToFullName: taskSpecifics.AssignedToExtraInfo?.FullNameDisplay,
            AssignedToPhoto: taskSpecifics.AssignedToExtraInfo?.Photo,
            UserStoryRef: taskSpecifics.UserStoryExtraInfo?.Ref ?? 0,
            UserStorySubject: taskSpecifics.UserStoryExtraInfo?.Subject,
            Version: taskSpecifics.Version
        );
    }
}

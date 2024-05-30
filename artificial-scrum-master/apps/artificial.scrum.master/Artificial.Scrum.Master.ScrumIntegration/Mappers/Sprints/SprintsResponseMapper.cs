using Artificial.Scrum.Master.ScrumIntegration.Features.Sprints;

namespace Artificial.Scrum.Master.ScrumIntegration.Mappers.Sprints;

internal interface ISprintsResponseMapper
{
    GetActiveSprintsResponse MapSprintsResponse(List<Sprint> sprints);
}

internal class SprintsResponseMapper : ISprintsResponseMapper
{
    public GetActiveSprintsResponse MapSprintsResponse(List<Sprint> sprints)
    {
        var mappedSprints = sprints.Select(sprint => new ActiveSprint
        {
            ProjectId = sprint.Project,
            ProjectName = sprint.ProjectExtraInfo?.Name,
            ProjectSlug = sprint.ProjectExtraInfo?.Slug,
            SprintId = sprint.Id,
            SprintName = sprint.Name,
            SprintSlug = sprint.Slug,
            EstimatedStart = sprint.EstimatedStart,
            EstimatedFinish = sprint.EstimatedFinish,
            UserStories = sprint.UserStories?.Select(userStory => new ActiveSprintUserStory
            {
                UserStoryId = userStory.Id,
                UserStoryName = userStory.Subject,
                StatusName = userStory.StatusExtraInfo?.Name,
                IsClosed = userStory.IsClosed,
                UserStoryRef = userStory.Ref,
                TotalPoints = userStory.TotalPoints ?? 0,
            })
        }).ToList();

        return new GetActiveSprintsResponse(Sprints: mappedSprints);
    }
}

using Artificial.Scrum.Master.ScrumIntegration.Features.UserStories;

namespace Artificial.Scrum.Master.ScrumIntegration.Mappers.UserStories;

internal interface IUserStoriesMapper
{
    GetUserStories MapUserStoriesResponse(List<UserStory> userStories);
}

internal class UserStoriesMapper : IUserStoriesMapper
{
    public GetUserStories MapUserStoriesResponse(List<UserStory> userStories)
    {
        var mappedUserStories = userStories.Select(us => new GetUserStoriesResponseElement
        {
            UserStoryId = us.Id,
            UserStorySubject = us.Subject,
            UserStoryRef = us.Ref,
            StatusName = us.StatusExtraInfo?.Name,
            IsClosed = us.StatusExtraInfo?.IsClosed ?? false,
            AssignedToUsername = us.AssignedToExtraInfo?.Username,
            AssignedToFullNameDisplay = us.AssignedToExtraInfo?.FullNameDisplay,
            AssignedToPhoto = us.AssignedToExtraInfo?.Photo,
            OwnerUsername = us.OwnerExtraInfo?.Username,
            OwnerFullNameDisplay = us.OwnerExtraInfo?.FullNameDisplay,
            OwnerPhoto = us.OwnerExtraInfo?.Photo,
            SprintId = us.Milestone,
            SprintSlug = us.MilestoneSlug,
            SprintName = us.MilestoneName,
            TotalPoints = us.TotalPoints,
        }).ToList();

        return new GetUserStories(UserStories: mappedUserStories);
    }
}

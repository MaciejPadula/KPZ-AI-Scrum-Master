using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.UserStoryDetails;

public interface IUserStoryDetailsService
{
    Task<UserStoryDetails> Handle(string storyId);
}
internal class UserStoryDetailsService(
    IAccessTokenProvider accessTokenProvider,
    IProjectHttpClientWrapper projectHttpClientWrapper,
    IUserAccessor userAccessor) : IUserStoryDetailsService
{
    private readonly IAccessTokenProvider _accessTokenProvider = accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper = projectHttpClientWrapper;
    private readonly IUserAccessor _userAccessor = userAccessor;

    public async Task<UserStoryDetails> Handle(string storyId)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var refreshToken = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var userStoryRequestResult = await _projectHttpClientWrapper.GetHttpRequest<UserStoryDetailsResponse>(userId,
            refreshToken,
            _ => $"userstories/{storyId}");

        return new UserStoryDetails
        {
            AssignedToName = userStoryRequestResult.AssignedToInfo?.Name,
            AssignedToPhotoUrl = userStoryRequestResult.AssignedToInfo?.PhotoUrl,
            Created = userStoryRequestResult.Created,
            Description = userStoryRequestResult.Description,
            IsStatusClosed = userStoryRequestResult.StatusInfo?.Closed ?? false,
            StatusColor = userStoryRequestResult.StatusInfo?.Color,
            StatusName = userStoryRequestResult.StatusInfo?.Name,
            Title = userStoryRequestResult.Title,
            Number = userStoryRequestResult.Number
        };
    }

}

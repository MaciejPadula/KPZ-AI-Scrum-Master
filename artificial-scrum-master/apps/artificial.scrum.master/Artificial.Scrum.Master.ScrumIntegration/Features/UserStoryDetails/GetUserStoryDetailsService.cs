using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.UserStoryDetails;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.UserStoryDetails;

public interface IGetUserStoryDetailsService
{
    Task<GetUserStoryDetailsResponse> Handle(string storyId);
}

internal class GetUserStoryDetailsService(
    IAccessTokenProvider accessTokenProvider,
    IProjectHttpClientWrapper projectHttpClientWrapper,
    IUserAccessor userAccessor) : IGetUserStoryDetailsService
{
    private readonly IAccessTokenProvider _accessTokenProvider = accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper = projectHttpClientWrapper;
    private readonly IUserAccessor _userAccessor = userAccessor;

    public async Task<GetUserStoryDetailsResponse> Handle(string storyId)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var refreshToken = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var userStoryRequestResult = await _projectHttpClientWrapper.GetHttpRequest<UserStorySpecifics>(userId,
            refreshToken,
            _ => $"userstories/{storyId}");

        return new GetUserStoryDetailsResponse(
            AssignedToName: userStoryRequestResult.AssignedToInfo?.Name,
            AssignedToPhotoUrl: userStoryRequestResult.AssignedToInfo?.PhotoUrl,
            Created: userStoryRequestResult.Created,
            Description: userStoryRequestResult.Description,
            DescriptionHtml: userStoryRequestResult.DescriptionHtml,
            IsStatusClosed: userStoryRequestResult.StatusInfo?.Closed ?? false,
            StatusColor: userStoryRequestResult.StatusInfo?.Color,
            StatusName: userStoryRequestResult.StatusInfo?.Name,
            Title: userStoryRequestResult.Title,
            Number: userStoryRequestResult.Number,
            Version: userStoryRequestResult.Version
        );
    }
}

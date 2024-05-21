using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.UserStoryDetails;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.EditUserStoryDetails;

internal interface IPatchStoryService
{
    Task<GetUserStoryDetailsResponse> Handle(string storyId, PatchStoryRequest patchStoryRequest);
}

internal class PatchStoryService : IPatchStoryService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IUserAccessor _userAccessor;

    public PatchStoryService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        IUserAccessor userAccessor)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _userAccessor = userAccessor;
    }

    public async Task<GetUserStoryDetailsResponse> Handle(string storyId, PatchStoryRequest patchStoryRequest)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var refreshToken = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var userStoryRequestResult =
            await _projectHttpClientWrapper.PatchHttpRequest<PatchStoryRequest, UserStorySpecifics>(
                userId,
                refreshToken,
                _ => $"userstories/{storyId}",
                patchStoryRequest);

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

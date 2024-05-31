using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.UserStory;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.UserStories;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.UserStories;

internal interface IGetUserStoriesService
{
    Task<GetUserStoriesResponse> Handle(string projectId, string sprintId);
}

internal class GetUserStoriesService : IGetUserStoriesService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IUserStoriesMapper _userStoriesMapper;
    private readonly IUserAccessor _userAccessor;

    public GetUserStoriesService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        IUserStoriesMapper userStoriesMapper,
        IUserAccessor userAccessor)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _userStoriesMapper = userStoriesMapper;
        _userAccessor = userAccessor;
    }

    public async Task<GetUserStoriesResponse> Handle(string projectId, string sprintId)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var userTokens = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var userStoriesRequestResponse = await _projectHttpClientWrapper.GetHttpRequest<List<UserStory>>(
            userId,
            userTokens,
            _ => $"userstories?project={projectId}&milestone={sprintId}");

        return _userStoriesMapper.MapUserStoriesResponse(userStoriesRequestResponse);
    }
}

using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.Tasks;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Tasks;

internal interface IGetStoryTasksService
{
    Task<GetStoryTasksResponse> Handle(string? userStoryId, string? sprintId);
}

internal class GetStoryTasksService : IGetStoryTasksService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly ITasksResponseMapper _tasksResponseMapper;
    private readonly IUserAccessor _userAccessor;

    public GetStoryTasksService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        ITasksResponseMapper tasksResponseMapper,
        IUserAccessor userAccessor)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _tasksResponseMapper = tasksResponseMapper;
        _userAccessor = userAccessor;
    }

    public async Task<GetStoryTasksResponse> Handle(string? userStoryId, string? sprintId)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var refreshToken = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var urlSuffix = string.IsNullOrEmpty(userStoryId)
            ? $"tasks?milestone={sprintId}&user_story=null"
            : $"tasks?user_story={userStoryId}";

        var userStoriesRequestResponse = await _projectHttpClientWrapper.GetHttpRequest<List<StoryTask>>(
            userId,
            refreshToken,
            _ => urlSuffix);

        return _tasksResponseMapper.MapUserStoriesResponse(userStoriesRequestResponse);
    }
}

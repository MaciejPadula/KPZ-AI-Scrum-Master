using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.Tasks;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Tasks;

internal interface IGetStoryTasksService
{
    Task<GetStoryTasksResponse> Handle(string userId, string? userStoryId, string? sprintId);
}

internal class GetStoryTasksService : IGetStoryTasksService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IJwtDecoder _jwtDecoder;
    private readonly ITasksResponseMapper _tasksResponseMapper;

    public GetStoryTasksService(IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper, IJwtDecoder jwtDecoder,
        ITasksResponseMapper tasksResponseMapper)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _jwtDecoder = jwtDecoder;
        _tasksResponseMapper = tasksResponseMapper;
    }

    public async Task<GetStoryTasksResponse> Handle(string userId, string? userStoryId, string? sprintId)
    {
        var userTokens = await _accessTokenProvider.ProvideOrThrow(userId);

        var memberId = _jwtDecoder.GetClaim(userTokens.AccessToken, "user_id");
        if (string.IsNullOrEmpty(memberId))
        {
            throw new ProjectRequestForbidException("User id not found in token");
        }

        var urlSuffix = string.IsNullOrEmpty(userStoryId)
            ? $"tasks?milestone={sprintId}&user_story=null"
            : $"tasks?user_story={userStoryId}";

        var userStoriesRequestResponse = await _projectHttpClientWrapper.GetHttpRequest<List<StoryTask>>(
            userId,
            userTokens,
            urlSuffix);

        return _tasksResponseMapper.MapUserStoriesResponse(userStoriesRequestResponse);
    }
}

using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.Task;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.UserStory;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.UserStories;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.GetStoriesWithTasks;

public interface IGetStoriesWithTasksService
{
    Task<GetStoriesWithTasksResponse> Handle(string projectId, string sprintId);
}

internal class GetStoriesWithTasksService : IGetStoriesWithTasksService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IUserStoriesWithTasksMapper _userStoriesMapper;
    private readonly IUserAccessor _userAccessor;

    public GetStoriesWithTasksService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        IUserStoriesWithTasksMapper userStoriesMapper,
        IUserAccessor userAccessor)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _userStoriesMapper = userStoriesMapper;
        _userAccessor = userAccessor;
    }

    public async Task<GetStoriesWithTasksResponse> Handle(string projectId, string sprintId)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var userTokens = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var userStoriesRequestTask = _projectHttpClientWrapper.GetHttpRequest<List<UserStory>>(
            userId,
            userTokens,
            _ => $"userstories?project={projectId}&milestone={sprintId}");
        var tasksRequestTask = _projectHttpClientWrapper.GetHttpRequest<List<StoryTask>>(
            userId,
            userTokens,
            _ => $"tasks?milestone={sprintId}&order_by=us_order&project={projectId}");

        await Task.WhenAll(userStoriesRequestTask, tasksRequestTask);
        return _userStoriesMapper.MapUserStoriesWithTasksResponse(
            userStoriesRequestTask.Result,
            tasksRequestTask.Result);
    }
}

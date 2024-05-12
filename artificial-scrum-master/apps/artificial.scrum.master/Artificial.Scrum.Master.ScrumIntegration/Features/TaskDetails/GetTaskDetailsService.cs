using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TaskDetails;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.Tasks;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.TaskDetails;

internal interface IGetTaskDetailsService
{
    Task<GetTaskDetailsResponse> Handle(string taskId);
}

internal class GetTaskDetailsService : IGetTaskDetailsService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly ITaskDetailsResponseMapper _taskDetailsResponseMapper;
    private readonly IUserAccessor _userAccessor;

    public GetTaskDetailsService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        ITaskDetailsResponseMapper taskDetailsResponseMapper,
        IUserAccessor userAccessor)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _taskDetailsResponseMapper = taskDetailsResponseMapper;
        _userAccessor = userAccessor;
    }

    public async Task<GetTaskDetailsResponse> Handle(string taskId)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var refreshToken = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var taskDetailsRequestResponse = await _projectHttpClientWrapper.GetHttpRequest<TaskSpecifics>(
            userId,
            refreshToken,
            _ => $"tasks/{taskId}");

        return _taskDetailsResponseMapper.MapTaskDetailsResponse(taskDetailsRequestResponse);
    }
}

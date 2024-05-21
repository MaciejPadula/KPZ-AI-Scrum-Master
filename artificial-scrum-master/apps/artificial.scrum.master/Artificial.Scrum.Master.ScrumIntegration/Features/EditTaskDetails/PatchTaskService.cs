using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TaskDetails;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.Tasks;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.EditTaskDetails;

internal interface IPatchTaskService
{
    Task<GetTaskDetailsResponse> Handle(string taskId, PatchTaskRequest patchTaskRequest);
}

internal class PatchTaskService : IPatchTaskService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly ITaskDetailsResponseMapper _taskDetailsResponseMapper;
    private readonly IUserAccessor _userAccessor;

    public PatchTaskService(
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

    public async Task<GetTaskDetailsResponse> Handle(string taskId, PatchTaskRequest patchTaskRequest)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var refreshToken = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var taskDetailsRequestResponse =
            await _projectHttpClientWrapper.PatchHttpRequest<PatchTaskRequest, TaskSpecifics>(
                userId,
                refreshToken,
                _ => $"tasks/{taskId}",
                patchTaskRequest);

        return _taskDetailsResponseMapper.MapTaskDetailsResponse(taskDetailsRequestResponse);
    }
}

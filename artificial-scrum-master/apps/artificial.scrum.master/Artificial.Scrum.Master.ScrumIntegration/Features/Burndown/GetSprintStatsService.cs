using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.BurnDown;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Burndown;

internal interface IGetSprintStatsService
{
    Task<GetSprintStatsResponse> Handle(string projectId, string sprintId);
}

internal class GetSprintStatsService : IGetSprintStatsService
{
    private readonly IUserAccessor _userAccessor;
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly ISprintStatsResponseMapper _sprintStatsResponseMapper;

    public GetSprintStatsService(IUserAccessor userAccessor,
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        ISprintStatsResponseMapper sprintStatsResponseMapper)
    {
        _userAccessor = userAccessor;
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _sprintStatsResponseMapper = sprintStatsResponseMapper;
    }

    public async Task<GetSprintStatsResponse> Handle(string projectId, string sprintId)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var refreshToken = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var sprintDataRequest = _projectHttpClientWrapper.GetHttpRequest<SprintData>(
            userId,
            refreshToken,
            _ => $"tasks/filters_data?milestone={sprintId}&project={projectId}");
        var sprintStatsRequest = _projectHttpClientWrapper.GetHttpRequest<SprintStats>(
            userId,
            refreshToken,
            _ => $"milestones/{sprintId}/stats");

        await Task.WhenAll(sprintDataRequest, sprintStatsRequest);
        return _sprintStatsResponseMapper.MapSprintStatsResponse(sprintStatsRequest.Result, sprintDataRequest.Result);
    }
}

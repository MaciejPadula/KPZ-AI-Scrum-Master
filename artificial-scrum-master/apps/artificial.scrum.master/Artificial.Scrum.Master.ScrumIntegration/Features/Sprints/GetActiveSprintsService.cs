using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.Sprints;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Sprints;

internal interface IGetActiveSprintsService
{
    Task<GetActiveSprintsResponse> Handle(string projectId);
}

internal class GetActiveSprintsService : IGetActiveSprintsService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly ISprintsResponseMapper _sprintsResponseMapper;
    private readonly IUserAccessor _userAccessor;

    public GetActiveSprintsService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        ISprintsResponseMapper sprintsResponseMapper,
        IUserAccessor userAccessor)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _sprintsResponseMapper = sprintsResponseMapper;
        _userAccessor = userAccessor;
    }

    public async Task<GetActiveSprintsResponse> Handle(string projectId)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var userTokens = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var sprintsRequestResponse = await _projectHttpClientWrapper.GetHttpRequest<List<Sprint>>(
            userId,
            userTokens,
            _ => $"milestones?project={projectId}&closed=false");

        return _sprintsResponseMapper.MapSprintsResponse(sprintsRequestResponse);
    }
}

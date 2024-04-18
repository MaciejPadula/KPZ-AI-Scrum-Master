using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.TimelineEvents;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Project;

internal interface IGetProjectTimeLineService
{
    Task<GetProjectTimeLine> Handle(string projectId);
}

internal class GetProjectTimeLineService : IGetProjectTimeLineService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly ITimeLineEventMapper _timeLineElementMapper;
    private readonly IUserAccessor _userAccessor;

    public GetProjectTimeLineService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        ITimeLineEventMapper timeLineElementMapper,
        IUserAccessor userAccessor)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _timeLineElementMapper = timeLineElementMapper;
        _userAccessor = userAccessor;
    }

    public async Task<GetProjectTimeLine> Handle(string projectId)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var refreshToken = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var projectTimeLineRequestResult =
            await _projectHttpClientWrapper.GetHttpRequest<List<TimeLineEventRoot>>(
                userId,
                refreshToken,
                _ => $"timeline/project/{projectId}");

        return _timeLineElementMapper.ParseProjectTimeLineElement(projectTimeLineRequestResult);
    }
}

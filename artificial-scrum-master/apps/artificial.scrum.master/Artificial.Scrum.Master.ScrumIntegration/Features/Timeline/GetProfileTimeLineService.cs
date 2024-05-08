using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TimeLine;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.TimelineEvents;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

internal interface IGetProfileTimeLineService
{
    Task<GetProfileTimeLineResponse> Handle();
}

internal class GetProfileTimeLineService : IGetProfileTimeLineService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly ITimeLineEventMapper _timeLineElementMapper;
    private readonly IUserAccessor _userAccessor;

    public GetProfileTimeLineService(
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

    public async Task<GetProfileTimeLineResponse> Handle()
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var refreshToken = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var profileTimeLineRequestResult =
            await _projectHttpClientWrapper.GetHttpRequest<List<TimeLineEventRoot>>(
                userId,
                refreshToken,
                user => $"timeline/profile/{user.UserId}");

        return _timeLineElementMapper.ParseProfileTimeLineElement(profileTimeLineRequestResult);
    }
}

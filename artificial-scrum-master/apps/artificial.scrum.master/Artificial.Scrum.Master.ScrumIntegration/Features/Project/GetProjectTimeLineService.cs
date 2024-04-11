using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.TimelineEvents;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Project;

internal interface IGetProjectTimeLineService
{
    Task<GetProjectTimeLineResponse> Handle(string userId, string projectId);
}

internal class GetProjectTimeLineService : IGetProjectTimeLineService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IJwtDecoder _jwtDecoder;
    private readonly ITimeLineEventMapper _timeLineElementMapper;

    public GetProjectTimeLineService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        IJwtDecoder jwtDecoder,
        ITimeLineEventMapper timeLineElementMapper)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _jwtDecoder = jwtDecoder;
        _timeLineElementMapper = timeLineElementMapper;
    }

    public async Task<GetProjectTimeLineResponse> Handle(string userId, string projectId)
    {
        var userTokens = await _accessTokenProvider.ProvideOrThrow(userId);

        var memberId = _jwtDecoder.GetClaim(userTokens.AccessToken, "user_id");
        if (string.IsNullOrEmpty(memberId))
        {
            throw new ProjectRequestForbidException("User id not found in token");
        }

        var projectTimeLineRequestResult =
            await _projectHttpClientWrapper.GetHttpRequest<List<TimeLineEventRoot>>(
                userId,
                userTokens,
                $"timeline/project/{projectId}");

        return _timeLineElementMapper.ParseProjectTimeLineElement(projectTimeLineRequestResult);
    }
}

using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

internal interface IGetProfileTimeLineService
{
    Task<GetProfileTimeLineResponse> Handle(string userId);
}

internal class GetProfileTimeLineService : IGetProfileTimeLineService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IJwtDecoder _jwtDecoder;
    private readonly ITimeLineEventParser _timeLineElementParser;

    public GetProfileTimeLineService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        IJwtDecoder jwtDecoder,
        ITimeLineEventParser timeLineElementParser)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _jwtDecoder = jwtDecoder;
        _timeLineElementParser = timeLineElementParser;
    }

    public async Task<GetProfileTimeLineResponse> Handle(string userId)
    {
        var userTokens = await _accessTokenProvider.ProvideOrThrow(userId);

        var memberId = _jwtDecoder.GetClaim(userTokens.AccessToken, "user_id");
        if (string.IsNullOrEmpty(memberId))
        {
            throw new ProjectRequestForbidException("User id not found in token");
        }

        var profileTimeLineRequestResult =
            await _projectHttpClientWrapper.GetHttpRequest<List<TimeLineEventRoot>>(
                userId,
                userTokens,
                $"timeline/profile/{memberId}");

        return _timeLineElementParser.ParseProfileTimeLineElement(profileTimeLineRequestResult);
    }
}

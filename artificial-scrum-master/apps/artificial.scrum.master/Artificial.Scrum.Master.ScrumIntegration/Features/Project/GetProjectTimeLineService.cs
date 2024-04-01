using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
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
    private readonly ITimeLineEventParser _timeLineElementParser;

    public GetProjectTimeLineService(
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

    public async Task<GetProjectTimeLineResponse> Handle(string userId, string projectId)
    {
        var userTokens = await _accessTokenProvider.ProvideOrThrow(userId);

        var memberId = _jwtDecoder.GetClaim(userTokens.AccessToken, "user_id");
        if (string.IsNullOrEmpty(memberId))
        {
            throw new ProjectRequestForbidException("User id not found in token");
        }

        var projectTimeLineRequestResult =
            await _projectHttpClientWrapper.GetHttpRequest<List<ProjectTimeLineElementRoot>>(
                userId,
                userTokens,
                $"timeline/project/{projectId}");

        return _timeLineElementParser.ParseProjectTimeLineElement(projectTimeLineRequestResult);
    }
}

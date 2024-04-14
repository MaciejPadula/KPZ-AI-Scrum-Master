using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.Sprints;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Sprints;

internal interface IGetActiveSprintsService
{
    Task<GetActiveSprints> Handle(string userId, string projectId);
}

internal class GetActiveSprintsService : IGetActiveSprintsService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IJwtDecoder _jwtDecoder;
    private readonly ISprintsResponseMapper _sprintsResponseMapper;

    public GetActiveSprintsService(IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper, IJwtDecoder jwtDecoder,
        ISprintsResponseMapper sprintsResponseMapper)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _jwtDecoder = jwtDecoder;
        _sprintsResponseMapper = sprintsResponseMapper;
    }

    public async Task<GetActiveSprints> Handle(string userId, string projectId)
    {
        var userTokens = await _accessTokenProvider.ProvideOrThrow(userId);

        var memberId = _jwtDecoder.GetClaim(userTokens.AccessToken, "user_id");
        if (string.IsNullOrEmpty(memberId))
        {
            throw new ProjectRequestForbidException("User id not found in token");
        }

        var sprintsRequestResponse = await _projectHttpClientWrapper.GetHttpRequest<List<Sprint>>(
            userId,
            userTokens,
            $"milestones?project={projectId}&closed=false");

        return _sprintsResponseMapper.MapSprintsResponse(sprintsRequestResponse);
    }
}

using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

internal interface IGetProfileTimeLineService
{
    Task<List<GetProfileTimeLineResponse>> Handle(string userId);
}

internal class GetProfileTimeLineService : IGetProfileTimeLineService
{
    private readonly IUserTokensRepository _userTokensRepository;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IJwtDecoder _jwtDecoder;

    public GetProfileTimeLineService(IUserTokensRepository userTokensRepository,
        IProjectHttpClientWrapper projectHttpClientWrapper, IJwtDecoder jwtDecoder)
    {
        _userTokensRepository = userTokensRepository;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _jwtDecoder = jwtDecoder;
    }

    public async Task<List<GetProfileTimeLineResponse>> Handle(string userId)
    {
        var userTokens = await _userTokensRepository.GetUserAccessTokens(userId);
        if (userTokens is null)
        {
            throw new ProjectRequestForbidException($"Credentials of user:{userId} not found");
        }

        var memberId = _jwtDecoder.GetClaim(userTokens.AccessToken, "user_id");
        if (string.IsNullOrEmpty(memberId))
        {
            throw new ProjectRequestForbidException("User id not found in token");
        }

        var profileTimeLineRequestResult = await _projectHttpClientWrapper.GetHttpRequest<List<Root>>(
            userId,
            userTokens,
            $"timeline/profile/{memberId}");

        return profileTimeLineRequestResult.Select(elem => new GetProfileTimeLineResponse
        {
        }).ToList();
    }
}

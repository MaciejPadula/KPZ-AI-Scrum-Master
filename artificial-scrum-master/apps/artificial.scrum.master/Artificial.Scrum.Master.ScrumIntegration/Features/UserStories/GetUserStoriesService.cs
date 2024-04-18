using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Features.Tasks;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.UserStories;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.UserStories;

internal interface IGetUserStoriesService
{
    Task<GetUserStories> Handle(string userId, string projectId, string sprintId);
}

internal class GetUserStoriesService : IGetUserStoriesService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IJwtDecoder _jwtDecoder;
    private readonly IUserStoriesMapper _userStoriesMapper;

    public GetUserStoriesService(IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper, IJwtDecoder jwtDecoder,
        IUserStoriesMapper userStoriesMapper)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _jwtDecoder = jwtDecoder;
        _userStoriesMapper = userStoriesMapper;
    }

    public async Task<GetUserStories> Handle(string userId, string projectId, string sprintId)
    {
        var userTokens = await _accessTokenProvider.ProvideOrThrow(userId);

        var memberId = _jwtDecoder.GetClaim(userTokens.AccessToken, "user_id");
        if (string.IsNullOrEmpty(memberId))
        {
            throw new ProjectRequestForbidException("User id not found in token");
        }

        var userStoriesRequestResponse = await _projectHttpClientWrapper.GetHttpRequest<List<UserStory>>(
            userId,
            userTokens,
            $"userstories?project={projectId}&milestone={sprintId}&is_closed=false");

        var firstOrDefaultStory = userStoriesRequestResponse.FirstOrDefault();
        var milestoneId = firstOrDefaultStory?.Milestone ?? (int.TryParse(sprintId, out var milestone) ? milestone : 0);
        userStoriesRequestResponse.Add(new UserStory
        {
            Id = null,
            Ref = null,
            Subject = "Storyless tasks",
            StatusExtraInfo = null,
            AssignedToExtraInfo = null,
            OwnerExtraInfo = null,
            Milestone = milestoneId,
            MilestoneSlug = firstOrDefaultStory?.MilestoneSlug,
            MilestoneName = firstOrDefaultStory?.MilestoneName,
            TotalPoints = null,
        });

        return _userStoriesMapper.MapUserStoriesResponse(userStoriesRequestResponse);
    }
}

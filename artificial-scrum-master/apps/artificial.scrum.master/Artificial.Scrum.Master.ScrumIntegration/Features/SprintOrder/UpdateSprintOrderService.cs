using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.SprintOrder;

internal interface IUpdateSprintOrderService
{
    Task Handle(SprintOrderRequest request);
}

internal class UpdateSprintOrderService : IUpdateSprintOrderService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IUserAccessor _userAccessor;

    public UpdateSprintOrderService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        IUserAccessor userAccessor)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _userAccessor = userAccessor;
    }

    public async Task Handle(SprintOrderRequest request)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var userTokens = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var apiUpdateSprintRequest = new ApiUpdateSprint
        {
            ProjectId = request.ProjectId,
            SprintId = request.SprintId,
            Stories = request.StoryIds
                .Select((storyId, index) => new BulkStory
                {
                    StoryId = storyId,
                    Order = index + 1
                }).ToList()
        };

        await _projectHttpClientWrapper.PostHttpRequest(
            userId,
            userTokens,
            _ => "userstories/bulk_update_milestone",
            apiUpdateSprintRequest);
    }
}

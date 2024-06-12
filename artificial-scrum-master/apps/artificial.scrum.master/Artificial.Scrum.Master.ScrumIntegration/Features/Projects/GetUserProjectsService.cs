using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Projects;

internal interface IGetUserProjectsService
{
    Task<GetUserProjectsResponse> Handle();
}

internal class GetUserProjectsService : IGetUserProjectsService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IUserAccessor _userAccessor;

    public GetUserProjectsService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        IUserAccessor userAccessor)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _userAccessor = userAccessor;
    }

    public async Task<GetUserProjectsResponse> Handle()
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var refreshToken = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var projectRequestResult = await _projectHttpClientWrapper.GetHttpRequest<List<Project>>(
            userId,
            refreshToken,
            user => $"projects?member={user.UserId}");

        return new GetUserProjectsResponse
        {
            Projects = projectRequestResult.Select(project => new UserProject
            {
                Id = project.Id,
                Name = project.Name,
                ModifiedDate = project.ModifiedDate,
                IsPrivate = project.IsPrivate,
                AmOwner = project.AmOwner,
                OwnerUsername = project.Owner.DisplayName
            }).ToList()
        };
    }
}

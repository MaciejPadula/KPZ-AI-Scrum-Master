using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Projects;

internal interface IGetUserProjectsService
{
    Task<GetUserProjectsResponse> Handle();
}

internal class GetUserProjectsService : IGetUserProjectsService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IJwtDecoder _jwtDecoder;
    private readonly IUserAccessor _userAccessor;

    public GetUserProjectsService(
        IAccessTokenProvider accessTokenProvider,
        IProjectHttpClientWrapper projectHttpClientWrapper,
        IJwtDecoder jwtDecoder,
        IUserAccessor userAccessor)
    {
        _accessTokenProvider = accessTokenProvider;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _jwtDecoder = jwtDecoder;
        _userAccessor = userAccessor;
    }

    public async Task<GetUserProjectsResponse> Handle()
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var userTokens = await _accessTokenProvider.ProvideOrThrow(userId);

        var memberId = _jwtDecoder.GetClaim(userTokens.AccessToken, "user_id");
        if (string.IsNullOrEmpty(memberId))
        {
            throw new ProjectRequestForbidException("User id not found in token");
        }

        var projectRequestResult = await _projectHttpClientWrapper.GetHttpRequest<List<Project>>(
            userId,
            userTokens,
            $"projects?member={memberId}");

        return new GetUserProjectsResponse
        {
            Projects = projectRequestResult.Select(project => new GetUserProjectsResponseElement
            {
                Id = project.Id,
                Name = project.Name,
                ModifiedDate = project.ModifiedDate,
                IsPrivate = project.IsPrivate,
                AmOwner = project.AmOwner,
                OwnerUsername = project.Owner.Username
            }).ToList()
        };
    }
}

using Artificial.Scrum.Master.ScrumProjectIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumProjectIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Features.Projects;

internal interface IGetUserProjectsService
{
    Task<IEnumerable<GetUserProjectsResponse>> Handle(string userId);
}

internal class GetUserProjectsService : IGetUserProjectsService
{
    private readonly IUserTokensRepository _userTokensRepository;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IJwtDecoder _jwtDecoder;

    public GetUserProjectsService(IUserTokensRepository userTokensRepository,
        IProjectHttpClientWrapper projectHttpClientWrapper, IJwtDecoder jwtDecoder)
    {
        _userTokensRepository = userTokensRepository;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _jwtDecoder = jwtDecoder;
    }

    public async Task<IEnumerable<GetUserProjectsResponse>> Handle(string userId)
    {
        var userTokens = await _userTokensRepository.GetUserAccessTokens(userId);
        if (userTokens is null)
        {
            throw new ProjectRequestForbidException($"Credentials of user:{userId} not found");
        }

        var projectRequestResult = await _projectHttpClientWrapper.GetHttpRequest<List<Project>>(
            userId,
            userTokens.Value,
            $"projects?member={_jwtDecoder.GetClaim(userTokens.Value.AccessToken, "user_id")}");

        return projectRequestResult.Select(project => new GetUserProjectsResponse
        {
            Id = project.Id,
            Name = project.Name,
            ModifiedDate = project.ModifiedDate,
            IsPrivate = project.IsPrivate,
            AmOwner = project.AmOwner,
            OwnerUsername = project.Owner.Username
        }).ToList();
    }
}

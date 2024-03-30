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
    private readonly IUserTokensRepository _userTokensRepository;
    private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
    private readonly IJwtDecoder _jwtDecoder;

    public GetProjectTimeLineService(IUserTokensRepository userTokensRepository,
        IProjectHttpClientWrapper projectHttpClientWrapper, IJwtDecoder jwtDecoder)
    {
        _userTokensRepository = userTokensRepository;
        _projectHttpClientWrapper = projectHttpClientWrapper;
        _jwtDecoder = jwtDecoder;
    }

    public Task<GetProjectTimeLineResponse> Handle(string userId, string projectId)
    {
        throw new NotImplementedException();
    }
}

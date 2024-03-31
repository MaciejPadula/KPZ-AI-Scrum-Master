using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Project;

internal interface IGetProjectTimeLineService
{
    Task<List<GetProjectTimeLineResponse>> Handle(string userId, string projectId);
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

    public async Task<List<GetProjectTimeLineResponse>> Handle(string userId, string projectId)
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

        var projectTimeLineRequestResult =
            await _projectHttpClientWrapper.GetHttpRequest<List<ProjectTimeLineElementRoot>>(
                userId,
                userTokens,
                $"timeline/project/{projectId}");

        return projectTimeLineRequestResult.Select(elem => new GetProjectTimeLineResponse
        {
            Id = elem.Id,
            EventType = elem.EventType,
            Created = elem.Created,
            ProjectId = elem.Project,
            Comment = elem.Data.Comment,
            CommentHtml = elem.Data.CommentHtml,
            TaskId = elem.Data.Task?.Id ?? -1,
            TaskSubject = elem.Data.Task?.Subject,
            UserStoryId = elem.Data.Task?.Userstory?.Id ?? -1,
            UserStorySubject = elem.Data.Task?.Userstory?.Subject,
            UserId = elem.Data.User.Id,
            UserName = elem.Data.User.Name,
            UserPhoto = elem.Data.User.Photo,
            UserUsername = elem.Data.User.Username,
            ProjectName = elem.Data.Project.Name,
            ValuesDiff = elem.Data.ValuesDiff,
        }).ToList();
    }
}

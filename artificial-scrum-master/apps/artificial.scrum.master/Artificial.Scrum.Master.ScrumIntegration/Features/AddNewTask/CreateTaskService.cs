using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TaskDetails;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.AddNewTask;

internal interface ICreateTaskService
{
    Task Handle(CreateTaskRequest createTaskRequest);
}

internal class CreateTaskService(
    IAccessTokenProvider _accessTokenProvider,
    IProjectHttpClientWrapper _projectHttpClientWrapper,
    IUserAccessor _userAccessor) : ICreateTaskService
{
    public async Task Handle(CreateTaskRequest createTaskRequest)
    {
        var userId = _userAccessor.UserId ?? throw new UnauthorizedAccessException();
        var refreshToken = await _accessTokenProvider.ProvideRefreshTokenOrThrow(userId);

        var request = new ApiCreateTaskRequest
        {
            Description = createTaskRequest.Description,
            Subject = createTaskRequest.Subject,
            Project = createTaskRequest.ProjectId,
            UserStory = createTaskRequest.UserStoryId,
            UsOrder = 1,
            TaskboardOrder = 1
        };

        await _projectHttpClientWrapper.PostHttpRequest(
            userId,
            refreshToken,
            _ => "tasks",
            request);
    }
}

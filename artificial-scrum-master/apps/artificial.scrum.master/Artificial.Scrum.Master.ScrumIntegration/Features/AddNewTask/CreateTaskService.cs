using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TaskDetails;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

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

        var request = new ApiCreateTaskRequest()
        {
            description = createTaskRequest.Description,
            subject = createTaskRequest.Subject,
            project = createTaskRequest.ProjectId,
            user_story = createTaskRequest.UserStoryId,
            us_order = 1,
            taskboard_order = 1
        };

        var response = await _projectHttpClientWrapper.PostHttpRequest<ApiCreateTaskRequest, TaskSpecifics>(
            userId,
            refreshToken,
            _ => $"tasks",
            request);
    }
}

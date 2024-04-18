using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetUserProjectSessions;

internal interface IGetUserProjectSessionsService
{
    Task<Result<GetUserProjectSessionsResponse>> Handle(int projectId);
}

internal class GetUserProjectSessionsService : IGetUserProjectSessionsService
{
    private readonly IUserAccessor _userAccessor;
    private readonly ISessionRepository _sessionRepository;

    public GetUserProjectSessionsService(
        IUserAccessor userAccessor,
        ISessionRepository sessionRepository)
    {
        _userAccessor = userAccessor;
        _sessionRepository = sessionRepository;
    }

    public async Task<Result<GetUserProjectSessionsResponse>> Handle(int projectId)
    {
        var userId = _userAccessor.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<GetUserProjectSessionsResponse>.OnError(new UnauthorizedAccessException());
        }

        var sessions = await _sessionRepository.GetUserProjectSessions(
            userId,
            projectId);

        return Result<GetUserProjectSessionsResponse>.OnSuccess(new GetUserProjectSessionsResponse(
            sessions
                .Select(x => new Session(x.Id, x.Name, x.ProjectId, x.OwnerId))
                .ToList()));
    }
}

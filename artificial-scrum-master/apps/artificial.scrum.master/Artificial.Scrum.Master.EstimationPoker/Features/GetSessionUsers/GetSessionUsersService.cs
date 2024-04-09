using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetSessionUsers;

internal interface IGetSessionUsersService
{
    Task<Result<GetSessionUsersResponse>> Handle(string sessionId);
}

internal class GetSessionUsersService : IGetSessionUsersService
{
    private readonly ISessionRepository _sessionRepository;

    public GetSessionUsersService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Result<GetSessionUsersResponse>> Handle(string sessionId)
    {
        var sessionExists = await _sessionRepository.SessionExists(sessionId);

        if (!sessionExists)
        {
            return Result<GetSessionUsersResponse>.OnError(new SessionNotFoundException(sessionId));
        }

        var users = await _sessionRepository.GetSessionUsers(sessionId);

        return Result<GetSessionUsersResponse>.OnSuccess(new GetSessionUsersResponse(users
            .Select(u => new SessionUser(u.UserName))
            .ToList()));
    }
}

using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
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
    private readonly ISessionUserRepository _sessionUserRepository;

    public GetSessionUsersService(
        ISessionRepository sessionRepository,
        ISessionUserRepository sessionUserRepository)
    {
        _sessionRepository = sessionRepository;
        _sessionUserRepository = sessionUserRepository;
    }

    public async Task<Result<GetSessionUsersResponse>> Handle(string sessionId)
    {
        var sessionExists = await _sessionRepository.SessionExists(sessionId);

        if (!sessionExists)
        {
            return Result<GetSessionUsersResponse>.OnError(new SessionNotFoundException(sessionId));
        }

        var users = await _sessionUserRepository.GetSessionUsers(sessionId);

        return Result<GetSessionUsersResponse>.OnSuccess(new GetSessionUsersResponse(users
            .Select(u => new SessionUser(
                u.Id,
                u.UserName))
            .ToList()));
    }
}

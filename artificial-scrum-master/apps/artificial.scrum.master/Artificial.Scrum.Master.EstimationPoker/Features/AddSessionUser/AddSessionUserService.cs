using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

namespace Artificial.Scrum.Master.EstimationPoker.Features.AddSessionUser;

internal interface IAddSessionUserService
{
    Task<Result> Handle(AddSessionUserRequest request);
}

internal class AddSessionUserService : IAddSessionUserService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ISessionUserRepository _sessionUserRepository;

    public AddSessionUserService(
        ISessionRepository sessionRepository,
        ISessionUserRepository sessionUserRepository)
    {
        _sessionRepository = sessionRepository;
        _sessionUserRepository = sessionUserRepository;
    }

    public async Task<Result> Handle(AddSessionUserRequest request)
    {
        var sessionExists = await _sessionRepository.SessionExists(request.SessionId);

        if (!sessionExists)
        {
            return Result.OnError(new SessionNotFoundException(request.SessionId));
        }

        await _sessionUserRepository.AddSessionUser(new SessionUserEntity(
            request.SessionId,
            request.UserName));

        return Result.OnSuccess();
    }
}

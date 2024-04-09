using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

namespace Artificial.Scrum.Master.EstimationPoker.Features.AddSessionUser;

internal interface IAddSessionUserService
{
    Task<Result> Handle(AddSessionUserRequest request);
}

internal class AddSessionUserService : IAddSessionUserService
{
    private readonly ISessionRepository _sessionRepository;

    public AddSessionUserService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Result> Handle(AddSessionUserRequest request)
    {
        var sessionExists = await _sessionRepository.SessionExists(request.SessionId);

        if (!sessionExists)
        {
            return Result.OnError(new SessionNotFoundException(request.SessionId));
        }

        await _sessionRepository.AddSessionUser(new Infrastructure.Models.SessionUserEntity(
            request.SessionId,
            request.UserName));

        return Result.OnSuccess();
    }
}

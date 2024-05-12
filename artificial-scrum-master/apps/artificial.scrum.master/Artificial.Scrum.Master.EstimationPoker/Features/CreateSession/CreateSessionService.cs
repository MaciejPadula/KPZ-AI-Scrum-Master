using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.SharedKernel;

namespace Artificial.Scrum.Master.EstimationPoker.Features.CreateSession;

internal interface ICreateSessionService
{
    Task<Result<CreateSessionResponse>> Handle(CreateSessionRequest request);
}

internal class CreateSessionService : ICreateSessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ISessionKeyGenerator _sessionKeyGenerator;
    private readonly IUserAccessor _userAccessor;

    public CreateSessionService(
        ISessionRepository sessionRepository,
        ISessionKeyGenerator sessionKeyGenerator,
        IUserAccessor userAccessor)
    {
        _sessionRepository = sessionRepository;
        _sessionKeyGenerator = sessionKeyGenerator;
        _userAccessor = userAccessor;
    }

    public async Task<Result<CreateSessionResponse>> Handle(CreateSessionRequest request)
    {
        var userId = _userAccessor.UserId;
        if (userId is null)
        {
            return Result<CreateSessionResponse>.OnError(new UserNotAuthenticatedException());
        }

        var sessionId = _sessionKeyGenerator.Key;

        await _sessionRepository.AddSession(new(
            sessionId,
            request.Name,
            request.ProjectId,
            userId));

        return Result<CreateSessionResponse>.OnSuccess(new CreateSessionResponse(sessionId));
    }
}

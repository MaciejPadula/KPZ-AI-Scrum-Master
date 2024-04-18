using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetSession;

internal interface IGetSessionService
{
    Task<Result<GetSessionResponse>> Handle(string sessionId);
}

internal class GetSessionService : IGetSessionService
{
    private readonly ISessionRepository _sessionRepository;

    public GetSessionService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Result<GetSessionResponse>> Handle(string sessionId)
    {
        var session = await _sessionRepository.GetSession(sessionId);
        if (!session.HasValue)
        {
            return Result<GetSessionResponse>.OnError(new SessionNotFoundException(sessionId));
        }

        return Result<GetSessionResponse>.OnSuccess(new GetSessionResponse(new(
            session.Value.Id,
            session.Value.Name,
            session.Value.ProjectId,
            session.Value.OwnerId)));
    }
}

using Artificial.Scrum.Master.Retrospectives.Features.Shared.Exceptions;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using Artificial.Scrum.Master.SharedKernel;

namespace Artificial.Scrum.Master.Retrospectives.Features.GetSprintSession;

internal interface IGetSessionHandler
{
    Task<Result<GetSessionResponse>> Handle(string sessionId);
}

internal class GetSessionHandler : IGetSessionHandler
{
    private readonly IRetroSessionRepository _retroSessionRepository;

    public GetSessionHandler(IRetroSessionRepository retroSessionRepository)
    {
        _retroSessionRepository = retroSessionRepository;
    }

    public async Task<Result<GetSessionResponse>> Handle(string sessionId)
    {
        var session = await _retroSessionRepository.GetSession(sessionId);

        if (!session.HasValue)
        {
            return new SessionNotFoundException(sessionId);
        }

        return new GetSessionResponse(session.Value.Name);
    }
}

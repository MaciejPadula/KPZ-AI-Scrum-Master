using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Exceptions;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using Artificial.Scrum.Master.SharedKernel;

namespace Artificial.Scrum.Master.Retrospectives.Features.CreateSessionIfNotExists;

internal interface ICreateSessionIfNotExistsHandler
{
    Task<Result<CreateSessionIfNotExistsResponse>> Handle(CreateSessionIfNotExistsRequest request);
}

internal class CreateSessionIfNotExistsHandler : ICreateSessionIfNotExistsHandler
{
    private readonly IRetroSessionRepository _retroSessionRepository;
    private readonly ISessionKeyGenerator _sessionKeyGenerator;
    private readonly IUserAccessor _userAccessor;

    public CreateSessionIfNotExistsHandler(
        IRetroSessionRepository retroSessionRepository,
        ISessionKeyGenerator sessionKeyGenerator,
        IUserAccessor userAccessor)
    {
        _retroSessionRepository = retroSessionRepository;
        _sessionKeyGenerator = sessionKeyGenerator;
        _userAccessor = userAccessor;
    }

    public async Task<Result<CreateSessionIfNotExistsResponse>> Handle(CreateSessionIfNotExistsRequest request)
    {
        var userId = _userAccessor.UserId;
        if (userId is null)
        {
            return new UserNotAuthenticatedException();
        }

        var session = await _retroSessionRepository.GetSession(request.SprintId);
        if (!session.HasValue)
        {
            session = new RetroSession(
                _sessionKeyGenerator.Key,
                request.Name,
                request.SprintId,
                request.ProjectId);
            await _retroSessionRepository.CreateSession(session.Value);
        }

        return new CreateSessionIfNotExistsResponse(session.Value.Id);
    }
}

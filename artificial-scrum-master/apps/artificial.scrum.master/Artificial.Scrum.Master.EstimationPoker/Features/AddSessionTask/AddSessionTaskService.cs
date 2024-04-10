using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;

namespace Artificial.Scrum.Master.EstimationPoker.Features.AddSessionTask;

internal interface IAddSessionTaskService
{
    Task<Result> Handle(AddSessionTaskRequest request);
}

internal class AddSessionTaskService : IAddSessionTaskService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ISessionTaskRepository _sessionTaskRepository;
    private readonly IUserAccessor _userAccessor;
    private readonly TimeProvider _timeProvider;

    public AddSessionTaskService(
        ISessionRepository sessionRepository,
        IUserAccessor userAccessor,
        TimeProvider timeProvider,
        ISessionTaskRepository sessionTaskRepository)
    {
        _sessionRepository = sessionRepository;
        _userAccessor = userAccessor;
        _timeProvider = timeProvider;
        _sessionTaskRepository = sessionTaskRepository;
    }

    public async Task<Result> Handle(AddSessionTaskRequest request)
    {
        var sessionExists = await _sessionRepository.SessionExists(request.SessionId);

        if (!sessionExists)
        {
            return Result.OnError(new SessionNotFoundException(request.SessionId));
        }

        var isValidated = await _sessionRepository.ValidateUserAccess(
            _userAccessor.UserId,
            request.SessionId);

        if (!isValidated)
        {
            return Result.OnError(new UserNotAuthorizedException(
                _userAccessor.UserId,
                request.SessionId));
        }

        await _sessionTaskRepository.AddSessionTask(new SessionTaskEntity(
            request.SessionId,
            request.Title,
            request.Description,
            _timeProvider.GetUtcNow().UtcDateTime));

        return Result.OnSuccess();
    }
}

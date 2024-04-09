using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Artificial.Scrum.Master.EstimationPoker.Features.AddSessionTask;

internal interface IAddSessionTaskService
{
    Task<Result> Handle(AddSessionTaskRequest request);
}

internal class AddSessionTaskService : IAddSessionTaskService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserAccessor _userAccessor;

    public AddSessionTaskService(
        ISessionRepository sessionRepository,
        IUserAccessor userAccessor)
    {
        _sessionRepository = sessionRepository;
        _userAccessor = userAccessor;
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
            return Result.OnError(new UnauthorizedAccessException());
        }

        await _sessionRepository.AddSessionTask(new SessionTaskEntity(
            request.SessionId,
            request.Title,
            request.Description));

        return Result.OnSuccess();
    }
}

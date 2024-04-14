using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetCurrentTask;

internal interface IGetCurrentTaskService
{
    Task<Result<GetCurrentTaskResponse>> Handle(string sessionId);
}

internal class GetCurrentTaskService : IGetCurrentTaskService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ISessionTaskRepository _sessionTaskRepository;

    public GetCurrentTaskService(
        ISessionRepository sessionRepository,
        ISessionTaskRepository sessionTaskRepository)
    {
        _sessionRepository = sessionRepository;
        _sessionTaskRepository = sessionTaskRepository;
    }

    public async Task<Result<GetCurrentTaskResponse>> Handle(string sessionId)
    {
        var sessionExists = await _sessionRepository.SessionExists(sessionId);

        if (!sessionExists)
        {
            return Result<GetCurrentTaskResponse>.OnError(new SessionNotFoundException(sessionId));
        }

        var currentTask = await _sessionTaskRepository.GetLatestTask(sessionId);

        if (!currentTask.HasValue)
        {
            return Result<GetCurrentTaskResponse>.OnError(new NoTasksInSessionException(sessionId));
        }

        return Result<GetCurrentTaskResponse>.OnSuccess(new GetCurrentTaskResponse(
            currentTask.Value.Id,
            currentTask.Value.Title,
            currentTask.Value.Description));
    }
}

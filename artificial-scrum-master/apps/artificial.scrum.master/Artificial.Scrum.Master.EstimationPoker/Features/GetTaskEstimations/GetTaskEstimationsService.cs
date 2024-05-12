using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.SharedKernel;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetTaskEstimations;

internal interface IGetTaskEstimationsService
{
    Task<Result<GetTaskEstimationsResponse>> Handle(int taskId);
}

internal class GetTaskEstimationsService : IGetTaskEstimationsService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ISessionTaskRepository _sessionTaskRepository;
    private readonly IUserAccessor _userAccessor;

    public GetTaskEstimationsService(
        ISessionTaskRepository sessionTaskRepository,
        ISessionRepository sessionRepository,
        IUserAccessor userAccessor)
    {
        _sessionTaskRepository = sessionTaskRepository;
        _sessionRepository = sessionRepository;
        _userAccessor = userAccessor;
    }

    public async Task<Result<GetTaskEstimationsResponse>> Handle(int taskId)
    {
        var userId = _userAccessor.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<GetTaskEstimationsResponse>.OnError(new UnauthorizedAccessException());
        }

        var task = await _sessionTaskRepository.GetTaskById(taskId);

        if (!task.HasValue)
        {
            return Result<GetTaskEstimationsResponse>.OnError(new TaskNotFoundException(taskId));
        }

        if (!await _sessionRepository.ValidateUserAccess(userId, task.Value.SessionId))
        {
            return Result<GetTaskEstimationsResponse>.OnError(new UserNotAuthorizedException(userId, task.Value.SessionId));
        }

        var estimations = await _sessionTaskRepository.GetTaskEstimations(taskId);
        var averageEstimation = estimations.Count > 0 ? estimations.Average(e => e.Value) : 0;

        return Result<GetTaskEstimationsResponse>.OnSuccess(
            new GetTaskEstimationsResponse(
                estimations
                    .Select(x => new TaskEstimation(
                        x.TaskId,
                        x.Username,
                        x.Value))
                    .ToList(),
                averageEstimation));
    }
}

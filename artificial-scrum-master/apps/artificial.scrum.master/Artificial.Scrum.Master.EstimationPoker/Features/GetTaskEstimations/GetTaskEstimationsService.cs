using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetTaskEstimations;

internal interface IGetTaskEstimationsService
{
    Task<Result<GetTaskEstimationsResponse>> Handle(int taskId);
}

internal class GetTaskEstimationsService : IGetTaskEstimationsService
{
    private readonly ISessionTaskRepository _sessionTaskRepository;

    public GetTaskEstimationsService(ISessionTaskRepository sessionTaskRepository)
    {
        _sessionTaskRepository = sessionTaskRepository;
    }

    public async Task<Result<GetTaskEstimationsResponse>> Handle(int taskId)
    {
        var taskExists = await _sessionTaskRepository.TaskExists(taskId);

        if (!taskExists)
        {
            return Result<GetTaskEstimationsResponse>.OnError(new TaskNotFoundException(taskId));
        }

        var estimations = await _sessionTaskRepository.GetTaskEstimations(taskId);
        var averageEstimation = estimations.Count > 0 ? estimations.Average(e => e.Value) : 0;

        return Result<GetTaskEstimationsResponse>.OnSuccess(
            new GetTaskEstimationsResponse(
                estimations
                    .Select(x => new TaskEstimation(
                        x.TaskId,
                        x.UserId,
                        x.Value))
                    .ToList(),
                averageEstimation));
    }
}

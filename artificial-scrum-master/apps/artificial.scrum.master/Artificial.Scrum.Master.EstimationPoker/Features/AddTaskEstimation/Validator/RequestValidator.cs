using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator.Estimation;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator;

internal interface IRequestValidator
{
    Task<Exception?> Validate(AddTaskEstimationRequest request);
}

internal class RequestValidator : IRequestValidator
{
    private readonly IEnumerable<IEstimationValidator> _estimationValidators;
    private readonly ISessionTaskRepository _sessionTaskRepository;

    public RequestValidator(
        IEnumerable<IEstimationValidator> estimationValidators,
        ISessionTaskRepository sessionTaskRepository)
    {
        _estimationValidators = estimationValidators;
        _sessionTaskRepository = sessionTaskRepository;
    }

    public async Task<Exception?> Validate(AddTaskEstimationRequest request)
    {
        var latestTask  = await _sessionTaskRepository.GetLatestTask(request.SessionId);

        if (latestTask?.Id != request.TaskId)
        {
            return new TaskIsNotLatestException(request.TaskId);
        }

        if (await _sessionTaskRepository.EstimationExists(request.Username, request.TaskId))
        {
            return new EstimationAlreadyExistsException(request.SessionId, request.Username, request.TaskId);
        }

        if (!_estimationValidators.Any(v => v.ValidateEstimationValue(request.EstimationValue)))
        {
            return new EstimationValidationException(request.EstimationValue);
        }

        return null;
    }
}

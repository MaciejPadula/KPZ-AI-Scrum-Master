using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator;
using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator.Estimation;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;

namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation;

internal interface IAddTaskEstimationService
{
    Task<Result> Handle(AddTaskEstimationRequest request);
}

internal class AddTaskEstimationService : IAddTaskEstimationService
{
    private readonly IRequestValidator _requestValidator;
    private readonly ISessionTaskRepository _sessionTaskRepository;

    public AddTaskEstimationService(
        IRequestValidator requestValidator,
        ISessionTaskRepository sessionTaskRepository)
    {
        _requestValidator = requestValidator;
        _sessionTaskRepository = sessionTaskRepository;
    }

    public async Task<Result> Handle(AddTaskEstimationRequest request)
    {
        var error = await _requestValidator.Validate(request);

        if (error is not null)
        {
            return Result.OnError(error);
        }

        await _sessionTaskRepository.AddTaskEstimation(new SessionTaskEstimationEntity(
            request.TaskId,
            request.UserId,
            request.EstimationValue));

        return Result.OnSuccess();
    }
}
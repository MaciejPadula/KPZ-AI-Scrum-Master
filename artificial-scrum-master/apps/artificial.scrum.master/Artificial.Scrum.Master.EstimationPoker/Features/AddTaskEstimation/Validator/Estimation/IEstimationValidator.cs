namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator.Estimation;

internal interface IEstimationValidator
{
    bool ValidateEstimationValue(decimal estimationValue);
}

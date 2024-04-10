namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator.Estimation;

internal class MinEstimationValidator : IEstimationValidator
{
    public bool ValidateEstimationValue(decimal estimationValue) => estimationValue > 0;
}

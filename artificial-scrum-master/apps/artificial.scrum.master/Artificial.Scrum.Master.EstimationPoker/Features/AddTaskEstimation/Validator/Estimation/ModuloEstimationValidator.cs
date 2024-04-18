namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator.Estimation;

internal class ModuloEstimationValidator : IEstimationValidator
{
    public bool ValidateEstimationValue(decimal estimationValue) => estimationValue % 0.5m == 0;
}

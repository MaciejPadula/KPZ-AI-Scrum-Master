namespace Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Exceptions;

internal class EstimationValidationException : Exception
{
    public decimal Estimation { get; private set; }

    public EstimationValidationException(decimal estimation)
    {
        Estimation = estimation;
    }

    public override string Message => $"Estimation {Estimation} is not valid";
}

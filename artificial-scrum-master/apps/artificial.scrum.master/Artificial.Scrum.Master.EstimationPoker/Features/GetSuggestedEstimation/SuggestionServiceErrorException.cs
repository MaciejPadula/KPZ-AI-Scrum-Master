namespace Artificial.Scrum.Master.EstimationPoker.Features.GetSuggestedEstimation;

internal class SuggestionServiceErrorException : Exception
{
    public SuggestionServiceErrorException() : base("Could not get value from external suggestions service")
    {
    }
}

namespace Artificial.Scrum.Master.ScrumIntegration.Features.SprintOrder;

internal class SprintOrderRequest
{
    public required List<int> StoryIds { get; set; }
    public int SprintId { get; set; }
    public int ProjectId { get; set; }
}

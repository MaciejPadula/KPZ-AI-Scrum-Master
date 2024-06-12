namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;

internal class CreateTaskRequest
{
    public int ProjectId { get; set; }
    public int? UserStoryId { get; set; }
    public required string Subject { get; set; }
    public string? Description { get; set; }
}

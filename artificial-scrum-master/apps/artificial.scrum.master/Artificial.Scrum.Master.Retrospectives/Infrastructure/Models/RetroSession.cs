namespace Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;

public readonly record struct RetroSession(
    string Id,
    string Name,
    int SprintId,
    int ProjectId);

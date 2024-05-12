namespace Artificial.Scrum.Master.Retrospectives.Features.CreateSessionIfNotExists;

internal record CreateSessionIfNotExistsRequest(
    string Name,
    int SprintId,
    int ProjectId);

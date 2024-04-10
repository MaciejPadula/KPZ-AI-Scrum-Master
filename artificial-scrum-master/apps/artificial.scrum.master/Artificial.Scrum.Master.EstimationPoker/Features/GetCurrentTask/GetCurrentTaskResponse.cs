namespace Artificial.Scrum.Master.EstimationPoker.Features.GetCurrentTask;

internal record GetCurrentTaskResponse(
    int Id,
    string Title,
    string Description);

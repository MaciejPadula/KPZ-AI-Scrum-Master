namespace Artificial.Scrum.Master.EstimationPoker.Features.AddSessionTask;

internal record AddSessionTaskRequest(
    string SessionId,
    string Title,
    string Description);

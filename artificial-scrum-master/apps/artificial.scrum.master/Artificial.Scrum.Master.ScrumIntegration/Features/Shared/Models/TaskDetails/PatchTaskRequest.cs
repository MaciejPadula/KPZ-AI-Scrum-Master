namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TaskDetails;

internal record PatchTaskRequest(
    int Version,
    string Description
);

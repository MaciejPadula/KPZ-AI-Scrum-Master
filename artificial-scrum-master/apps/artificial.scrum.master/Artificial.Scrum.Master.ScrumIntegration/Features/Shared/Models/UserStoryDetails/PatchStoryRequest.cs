namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.UserStoryDetails;

internal record PatchStoryRequest(
    int Version,
    string Description
);

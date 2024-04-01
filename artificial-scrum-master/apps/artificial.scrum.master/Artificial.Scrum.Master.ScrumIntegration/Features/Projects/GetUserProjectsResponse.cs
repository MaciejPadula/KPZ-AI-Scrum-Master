namespace Artificial.Scrum.Master.ScrumIntegration.Features.Projects;

internal readonly record struct GetUserProjectsResponse(
    int Id,
    string Name,
    DateTime ModifiedDate,
    bool IsPrivate,
    bool AmOwner,
    string OwnerUsername
);

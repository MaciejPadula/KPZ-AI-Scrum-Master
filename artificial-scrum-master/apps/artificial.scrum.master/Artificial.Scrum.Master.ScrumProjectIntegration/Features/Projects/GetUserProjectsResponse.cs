namespace Artificial.Scrum.Master.ScrumProjectIntegration.Features.Projects;

public readonly record struct GetUserProjectsResponse(
    int Id,
    string Name,
    DateTime ModifiedDate,
    bool IsPrivate,
    bool AmOwner,
    string OwnerUsername
);

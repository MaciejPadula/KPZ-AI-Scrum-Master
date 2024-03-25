namespace Artificial.Scrum.Master.ScrumProjectIntegration.Features.Projects
{
    public readonly record struct GetUserProjectsResponse(
        int Id,
        string Name,
        string Slug,
        string Description,
        DateTime ModifiedDate,
        string OwnerUsername,
        bool IsPrivate
    );
}

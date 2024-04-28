namespace Artificial.Scrum.Master.ScrumIntegration.Features.Burndown;

public class SprintData
{
    public List<Role>? Roles { get; set; }
}

public class Role
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

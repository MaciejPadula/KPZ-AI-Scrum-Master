using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Features.Projects;

public class Project
{
    public int Id { get; set; }
    public required string Name { get; set; }
    [JsonPropertyName("modified_date")] public DateTime ModifiedDate { get; set; }
    public required Owner Owner { get; set; }
    [JsonPropertyName("is_private")] public bool IsPrivate { get; set; }
    [JsonPropertyName("i_am_owner")] public bool AmOwner { get; set; }
}

public class Owner
{
    public required string Username { get; set; }
}

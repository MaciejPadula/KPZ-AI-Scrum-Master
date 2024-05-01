using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.UserStoryDetails;
public class UserStoryDetails
{
    public DateTime? Created { get; set; }
    public string? Description { get; set; }
    public string? AssignedToName { get; set; }
    public string? AssignedToPhotoUrl { get; set; }
    public string? StatusColor { get; set; }
    public bool IsStatusClosed { get; set; }
    public string? StatusName { get; set; }
    public string? Title { get; set; }
    public int ? Number { get; set; }
}

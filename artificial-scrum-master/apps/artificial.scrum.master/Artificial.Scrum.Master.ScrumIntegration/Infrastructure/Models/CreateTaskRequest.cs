using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;
internal class CreateTaskRequest
{
    public string? Description { get; set; }
    public string? Subject { get; set; }
    public int? ProjectId { get; set; }
    public int? UserStoryId { get; set; }
}

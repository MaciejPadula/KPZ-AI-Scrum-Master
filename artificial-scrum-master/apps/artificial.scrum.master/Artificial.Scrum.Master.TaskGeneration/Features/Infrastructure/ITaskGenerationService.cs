using Artificial.Scrum.Master.TaskGeneration.Features.GenerateTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.TaskGeneration.Features.Infrastructure;
public interface ITaskGenerationService
{
    Task<GenerateTasksResponse?> GenerateTasks(GenerateTasksRequest request);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.TaskGeneration.Features.GenerateTasks;
public readonly record struct GenerateTasksResponse
{
    public List<Task> Tasks { get; init; }
}

public readonly record struct Task
{
    public string Title { get; init; }
    public string Description { get; init; }
}

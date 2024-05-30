using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.TaskGeneration.Features.GenerateTasks;
public readonly record struct GenerateTasksResponse(List<Task> Tasks);

public readonly record struct Task(string Title, string Description);

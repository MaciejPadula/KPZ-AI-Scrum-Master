using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.TaskGeneration.Exceptions;
internal class GenerateTasksFailException : Exception
{
    public GenerateTasksFailException(string message) : base(message)
    {
    }
}

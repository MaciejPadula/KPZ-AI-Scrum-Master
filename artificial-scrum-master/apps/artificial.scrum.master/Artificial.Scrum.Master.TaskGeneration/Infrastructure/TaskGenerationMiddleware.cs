using Artificial.Scrum.Master.TaskGeneration.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.TaskGeneration.Infrastructure;
internal class TaskGenerationMiddleware(ILogger<TaskGenerationMiddleware> logger)
{
    private readonly ILogger<TaskGenerationMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(ex.Message);
        }
        catch (GenerateTasksFailException ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(ex.Message);
            _logger.LogError(ex, ex.Message);
        }
    }
}

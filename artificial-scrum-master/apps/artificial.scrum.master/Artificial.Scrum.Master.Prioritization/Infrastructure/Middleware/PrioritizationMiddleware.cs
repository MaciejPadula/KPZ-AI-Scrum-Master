using Artificial.Scrum.Master.Prioritization.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Artificial.Scrum.Master.Prioritization.Infrastructure.Middleware;

internal class PrioritizationMiddleware : IMiddleware
{
    private readonly ILogger<PrioritizationMiddleware> _logger;

    public PrioritizationMiddleware(ILogger<PrioritizationMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (GeneratePrioritySuggestionFailException ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(ex.Message);
            _logger.LogError(ex, ex.Message);
        }
    }
}

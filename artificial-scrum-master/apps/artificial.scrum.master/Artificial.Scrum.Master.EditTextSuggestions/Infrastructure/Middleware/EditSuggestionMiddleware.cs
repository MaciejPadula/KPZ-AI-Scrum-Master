using Artificial.Scrum.Master.EditTextSuggestions.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Artificial.Scrum.Master.EditTextSuggestions.Infrastructure.Middleware;

internal class EditSuggestionMiddleware : IMiddleware
{
    private readonly ILogger<EditSuggestionMiddleware> _logger;

    public EditSuggestionMiddleware(ILogger<EditSuggestionMiddleware> logger)
    {
        _logger = logger;
    }

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
        catch (GenerateSuggestionFailException ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(ex.Message);
            _logger.LogError(ex, ex.Message);
        }
    }
}

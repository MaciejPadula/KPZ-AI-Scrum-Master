using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Artificial.Scrum.Master.EstimationPoker.Features.Shared;

internal static class HttpContextExtensions
{
    public static async Task HandleResult(this HttpContext httpContext, Result result)
    {
        if (result.IsSuccess)
        {
            return;
        }

        var exception = result.Error?.Exception;

        httpContext.Response.StatusCode = exception switch
        {
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            SessionNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        await httpContext.Response.WriteAsJsonAsync(new { message = exception?.Message });
    }

    public static async Task HandleResult<T>(this HttpContext httpContext, Result<T> result)
    {
        if (result.IsSuccess)
        {
            await httpContext.Response.WriteAsJsonAsync(result.Value);
            return;
        }
        
        await httpContext.HandleResult((Result)result);
    }
}

using Artificial.Scrum.Master.Retrospectives.Features.CreateSessionCard;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Exceptions;
using Artificial.Scrum.Master.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace Artificial.Scrum.Master.Retrospectives.Features.Shared;

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
            SessionNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            InvalidCardTypeException => StatusCodes.Status400BadRequest,
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

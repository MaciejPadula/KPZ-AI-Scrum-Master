using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Middleware
{
    internal class ScrumIntegrationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ProjectRequestForbidException ex)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
            catch (ProjectResourceNotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
            catch (RefreshTokenExpiredException ex)
            {
                context.Response.StatusCode = 410;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
            catch (ProjectRequestFailedException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
        }
    }
}

using Artificial.Scrum.Master.ScrumProjectIntegration.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Middleware
{
    internal class ScrumProjectIntegrationMiddleware : IMiddleware
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
            catch (ProjectRequestFailedException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
        }
    }
}

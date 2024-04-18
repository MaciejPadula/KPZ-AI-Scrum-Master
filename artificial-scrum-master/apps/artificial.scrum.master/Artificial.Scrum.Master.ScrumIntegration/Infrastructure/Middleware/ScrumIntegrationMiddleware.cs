using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Middleware
{
    internal class ScrumIntegrationMiddleware : IMiddleware
    {
        private readonly ILogger<ScrumIntegrationMiddleware> _logger;

        public ScrumIntegrationMiddleware(ILogger<ScrumIntegrationMiddleware> logger)
        {
            _logger = logger;
        }

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
                _logger.LogError(ex, ex.Message);
            }
            catch (ProjectResourceNotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            catch (RefreshTokenExpiredException ex)
            {
                context.Response.StatusCode = 410;
                await context.Response.WriteAsJsonAsync(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            catch (ProjectRequestFailedException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}

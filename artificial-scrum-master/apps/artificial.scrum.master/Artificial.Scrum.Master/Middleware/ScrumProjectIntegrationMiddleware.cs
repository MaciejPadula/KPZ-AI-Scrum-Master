using Artificial.Scrum.Master.ScrumProjectIntegration.Exceptions;

namespace Artificial.Scrum.Master.Middleware
{
    public class ScrumProjectIntegrationMiddleware : IMiddleware
    {
        private readonly ILogger<ScrumProjectIntegrationMiddleware> _logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (RequestFailedException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync("Something went wrong...");
            }
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Artificial.Scrum.Master.ScrumProjectIntegration;

public static class ScrumProjectIntegrationEndpoints
{
    public static void RegisterScrumProjectIntegrationEndpoints(this IEndpointRouteBuilder routes)
    {
        // routes.MapGet(
        //     "/api/test",
        //     async (HttpContext context, IGetUserSettingsService service) =>
        //     {
        //         var result = await service.Handle(userId);
        //         var response = new GerUserSettingsResponse(result.TaigaApiKey);
        //
        //         await context.Response.WriteAsJsonAsync(response);
        //     });
    }
}

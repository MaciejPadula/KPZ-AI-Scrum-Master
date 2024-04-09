using Artificial.Scrum.Master.UserSettings.Features.GetUserSettings;
using Artificial.Scrum.Master.UserSettings.Features.SetTaigaAccess;
using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.UserSettings;

public static class UserSettingsModule
{
    public static void AddUserSettingsModule(this IServiceCollection services)
    {
        services.AddTransient<IGetUserSettingsService, GetUserSettingsService>();
        services.AddTransient<ISetTaigaAccessService, SetTaigaAccessService>();
    }

    public static void RegisterUserSettingsEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/api/user-settings",
            async (HttpContext context, IGetUserSettingsService service) =>
            {
                var response = await service.Handle();
                await context.Response.WriteAsJsonAsync(response);
            });

        routes.MapPost(
            "/api/user-settings/set-taiga-access",
            async (SetTaigaAccessRequest settings, HttpContext context, ISetTaigaAccessService service) =>
            {
                await service.Handle(settings);
            });
    }
}

using Artificial.Scrum.Master.UserSettings.Features.GetUserSettings;
using Artificial.Scrum.Master.UserSettings.Features.SetUserSettings;
using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.UserSettings;

public static class UserSettingsModule
{
    public static void AddUserSettings(this IServiceCollection services)
    {
        services.AddTransient<IGetUserSettingsService, GetUserSettingsService>();
        services.AddTransient<ISetUserSettingsService, SetUserSettingsService>();
    }

    public static void RegisterUserSettingsEndpoints(this IEndpointRouteBuilder routes)
    {
        string userId = "1";

        routes.MapGet(
            "/api/user-settings",
            async (HttpContext context, IGetUserSettingsService service) =>
            {
                var result = await service.Handle(userId);
                var response = new GerUserSettingsResponse(result.TaigaAccess);

                await context.Response.WriteAsJsonAsync(response);
            });

        routes.MapPost(
            "/api/user-settings",
            async (SetUserSettingsRequest settings, HttpContext context, ISetUserSettingsService service) =>
            {
                await service.Handle(userId, new Settings(settings.TaigaAccess));
            });
    }
}

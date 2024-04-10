using Artificial.Scrum.Master.User.Features.Authorization;
using Artificial.Scrum.Master.User.Features.ExternalSignIn;
using Artificial.Scrum.Master.User.Infrastructure.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Artificial.Scrum.Master.User
{
    public static class UserModule
    {
        public static void AddUserModule(this IServiceCollection services, IConfiguration configuration)
        {
            var encryptionKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");

            services.AddTransient<IExternalSignInService, ExternalSignInService>();
            services.AddTransient<IJwtAuthorizationService, JwtAuthorizationService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };

                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["AuthToken"];
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost(
                "/api/user/google-sign-in",
                async (HttpContext context, IExternalSignInService externalSignInService) =>
                {
                    var tokenResponse = await externalSignInService.Handle(
                        context.Request.Headers.Authorization.ToString().Replace("Bearer ", "")
                            );

                    if (tokenResponse.Success && !string.IsNullOrEmpty(tokenResponse.Token))
                    {

                        context.Response.Cookies.Append("AuthToken", tokenResponse.Token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        });

                        await context.Response.WriteAsJsonAsync(new
                        {
                            success = true
                        });
                    }
                    else
                    {
                        await context.Response.WriteAsJsonAsync(new
                        {
                            success = false
                        });
                    }

                });

            routes.MapPost(
                "/api/user/logout",
                async (HttpContext context) =>
                {
                    context.Response.Cookies.Delete("AuthToken");

                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = true
                    });
                });

            routes.MapGet(
            "/api/user/user-info",
            async (HttpContext context, IAuthorizationService service) =>
            {
                var userInfo = service.Handle();

                await context.Response.WriteAsJsonAsync(new
                {
                    isAuthorized = !string.IsNullOrEmpty(userInfo.UserId),
                    userId = userInfo.UserId,
                    userName = userInfo.UserName,
                    userPhotoUrl = userInfo.PhotoUrl
                });
            });
        }
    }
}

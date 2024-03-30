using Artificial.Scrum.Master.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string sqlConnectionString)
    {
        services.AddTransient<IDbConnectionFactory>(_ => new SqlDbConnectionFactory(sqlConnectionString));
        services.AddTransient(_ => TimeProvider.System);

        services.AddTransient<IUserAccessor, MockedUserAccessor>();

        return services;
    }
}

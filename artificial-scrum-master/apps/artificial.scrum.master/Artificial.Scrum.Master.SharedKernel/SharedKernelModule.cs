using Microsoft.Extensions.DependencyInjection;

namespace Artificial.Scrum.Master.SharedKernel;

public static class SharedKernelModule
{
    public static void AddSharedKernel(this IServiceCollection services)
    {
        services.AddTransient<ISessionKeyGenerator, SessionKeyGenerator>();
    }
}

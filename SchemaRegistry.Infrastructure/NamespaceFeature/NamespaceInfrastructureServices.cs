using Microsoft.Extensions.DependencyInjection;
using SchemaRegistry.Application.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

internal static class NamespaceInfrastructureServices
{
    public static IServiceCollection AddNamespaceInfrastructureServices(
        this IServiceCollection services
    )
    {
        services.AddScoped<ICreateNamespaceService, CreateNamespaceService>();

        return services;
    }
}

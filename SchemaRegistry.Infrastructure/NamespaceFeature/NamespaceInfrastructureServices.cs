using Microsoft.Extensions.DependencyInjection;
using SchemaRegistry.Application.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

internal static class NamespaceInfrastructureServices
{
    public static IServiceCollection AddNamespaceOrleansServices(this IServiceCollection services)
    {
        services
            .AddScoped<ICreateNamespaceService, NamespaceGrainService>()
            .AddScoped<IDeleteNamespaceService, NamespaceGrainService>()
            .AddScoped<IRestoreNamespaceService, NamespaceGrainService>();

        return services;
    }
}

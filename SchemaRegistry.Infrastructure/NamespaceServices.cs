using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public static class SchemaRegistryInfrastructureServices
{
    public static IServiceCollection AddSchemaRegistryInfrastructureServices(
        this IServiceCollection services
    )
    {
        return services
            .AddNamespaceInfrastructureServices()
            .ConfigureMarten(options =>
            {
                options.Events.AddEventType<NamespaceWasCreated>();
            });
    }
}

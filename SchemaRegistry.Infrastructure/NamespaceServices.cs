using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public static class SchemaRegistryInfrastructureServices
{
    public static IServiceCollection AddSchemaRegistryOrleansServices(
        this IServiceCollection services
    )
    {
        return services
            .AddNamespaceOrleansServices()
            .ConfigureMarten(options =>
            {
                options.Events.AddEventType<NamespaceWasCreated>();
                options.Events.AddEventType<NamespaceDescriptionsWereUpdated>();
                options.Events.AddEventType<NamespaceWasSoftDeleted>();
                options.Events.AddEventType<NamespaceWasRestored>();
                options.Events.AddEventType<NamespaceWasPermanentlyDeleted>();
            });
    }
}

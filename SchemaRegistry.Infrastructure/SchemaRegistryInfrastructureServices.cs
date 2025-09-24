using Marten;
using Microsoft.Extensions.DependencyInjection;
using SchemaRegistry.Infrastructure.NamespaceFeature;

namespace SchemaRegistry.Infrastructure;

public static class SchemaRegistryInfrastructureServices
{
    public static IServiceCollection AddSchemaRegistryOrleansServices(
        this IServiceCollection services
    )
    {
        return services.AddNamespaceOrleansServices();
    }
}

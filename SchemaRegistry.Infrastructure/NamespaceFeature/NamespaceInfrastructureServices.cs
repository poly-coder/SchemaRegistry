using Marten;
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
            .AddScoped<IUpdateNamespaceDescriptionsService, NamespaceGrainService>()
            .AddScoped<IUpdateNamespaceDocumentationService, NamespaceGrainService>()
            .AddScoped<IRestoreNamespaceService, NamespaceGrainService>()
            .ConfigureMarten(options =>
            {
                options.Events.AddEventType<NamespaceWasCreated>();
                options.Events.AddEventType<NamespaceDescriptionsWereUpdated>();
                options.Events.AddEventType<NamespaceDocumentationWasUpdated>();
                options.Events.AddEventType<NamespaceWasDeleted>();
                options.Events.AddEventType<NamespaceWasRestored>();
            });

        return services;
    }
}

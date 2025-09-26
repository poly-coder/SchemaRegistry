using Marten;
using Microsoft.Extensions.DependencyInjection;
using SchemaRegistry.Application.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

internal static class NamespaceInfrastructureServices
{
    public static IServiceCollection AddNamespaceOrleansServices(this IServiceCollection services)
    {
        services
            .AddScoped<ICreateNamespaceUseCase, NamespaceGrainUseCase>()
            .AddScoped<IDeleteNamespaceUseCase, NamespaceGrainUseCase>()
            .AddScoped<IUpdateNamespaceDescriptionsUseCase, NamespaceGrainUseCase>()
            .AddScoped<IUpdateNamespaceDocumentationUseCase, NamespaceGrainUseCase>()
            .AddScoped<IRestoreNamespaceUseCase, NamespaceGrainUseCase>()
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

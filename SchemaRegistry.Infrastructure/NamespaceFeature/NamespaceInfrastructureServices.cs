using Marten;
using Microsoft.Extensions.DependencyInjection;
using SchemaRegistry.Application.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

internal static class NamespaceInfrastructureServices
{
    public static IServiceCollection AddNamespaceOrleansServices(this IServiceCollection services)
    {
        services
            .AddScoped<ICreateNamespaceUseCase, NamespaceGrainUseCases>()
            .AddScoped<IDeleteNamespaceUseCase, NamespaceGrainUseCases>()
            .AddScoped<IUpdateNamespaceDescriptionsUseCase, NamespaceGrainUseCases>()
            .AddScoped<IUpdateNamespaceDocumentationUseCase, NamespaceGrainUseCases>()
            .AddScoped<IRestoreNamespaceUseCase, NamespaceGrainUseCases>()
            .AddScoped<IGetNamespaceByIdUseCase, NamespaceGrainUseCases>()
            .AddScoped<IGetNamespaceByIdsUseCase, GetNamespaceByIdsUseCase>()
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

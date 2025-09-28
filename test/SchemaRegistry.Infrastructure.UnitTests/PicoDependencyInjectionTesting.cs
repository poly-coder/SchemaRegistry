using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Pico.Reflection;

namespace Pico.DependencyInjection.Testing;

public static class PicoDependencyInjectionTestingExtensions
{
    public static SettingsTask VerifyRegisteredServices(
        Action<IServiceCollection> configureServices,
        [CallerFilePath] string sourceFile = ""
    )
    {
        var registered = CaptureRegisteredServices(configureServices);

        // ReSharper disable once ExplicitCallerInfoArgument
        return Verify(new { RegisteredServices = registered }, sourceFile: sourceFile);
    }

    public static IReadOnlyList<ServiceDescriptorTestInfo> CaptureRegisteredServices(
        this IServiceCollection services,
        Action<IServiceCollection> configureServices
    )
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureServices);

        configureServices(services);

        return services
            .Select(d => new ServiceDescriptorTestInfo(
                Lifetime: d.Lifetime,
                ServiceType: d.ServiceType.ToDisplayName(),
                ServiceKey: d.IsKeyedService ? $"{d.ServiceKey}" : null,
                ImplementationType: d.IsKeyedService
                    ? d.KeyedImplementationType?.ToDisplayName()
                    : d.ImplementationType?.ToDisplayName(),
                ImplementationInstance: d.IsKeyedService
                    ? d.KeyedImplementationInstance?.GetType().ToDisplayName()
                    : d.ImplementationInstance?.GetType().ToDisplayName()
            ))
            .OrderBy(d => d.Lifetime)
            .ThenBy(d => d.ServiceType)
            .ThenBy(d => d.ServiceKey)
            .ToArray();
    }

    public static IReadOnlyList<ServiceDescriptorTestInfo> CaptureRegisteredServices(
        Action<IServiceCollection> configureServices
    )
    {
        ArgumentNullException.ThrowIfNull(configureServices);

        var services = new ServiceCollection();

        configureServices(services);

        return services
            .Select(d => new ServiceDescriptorTestInfo(
                d.Lifetime,
                d.ServiceType.ToDisplayName(),
                d.IsKeyedService ? $"{d.ServiceKey}" : null
            ))
            .OrderBy(d => d.Lifetime)
            .ThenBy(d => d.ServiceType)
            .ThenBy(d => d.ServiceKey)
            .ToArray();
    }
}

public sealed record ServiceDescriptorTestInfo(
    ServiceLifetime Lifetime,
    string ServiceType,
    string? ServiceKey,
    string? ImplementationType = null,
    string? ImplementationInstance = null,
    string? ImplementationMethod = null
);

public sealed record RegisteredServicesTestInfo(
    IReadOnlyList<ServiceDescriptorTestInfo> AddedServices,
    IReadOnlyList<ServiceDescriptorTestInfo> RemovedServices
);

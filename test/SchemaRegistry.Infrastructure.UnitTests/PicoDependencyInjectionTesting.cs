using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Pico.Reflection;

namespace Pico.DependencyInjection.Testing;

public static class PicoDependencyInjectionTestingExtensions
{
    //public static SettingsTask VerifyRegisteredServices(
    //    Action<IServiceCollection> configureServices,
    //    [CallerFilePath] string sourceFile = ""
    //)
    //{
    //    var registered = CaptureRegisteredServices(configureServices);

    //    // ReSharper disable once ExplicitCallerInfoArgument
    //    return Verify(new { RegisteredServices = registered }, sourceFile: sourceFile);
    //}

    public static CaptureRegisteredServicesTestResult ForTesting(
        this CaptureRegisteredServicesResult capture,
        ToDisplayNameOptions? options = null
    )
    {
        options ??= ToDisplayNameOptions.Default;

        return new(
            Added: capture
                .Added.Select(e => MapToTestInfo(e, options))
                .OrderBy(d => d.Lifetime)
                .ThenBy(d => d.ServiceType)
                .ThenBy(d => d.ServiceKey)
                .ToArray(),
            Removed: capture
                .Removed.Select(e => MapToTestInfo(e, options))
                .OrderBy(d => d.Lifetime)
                .ThenBy(d => d.ServiceType)
                .ThenBy(d => d.ServiceKey)
                .ToArray()
        );
    }

    public static ServiceDescriptorTestInfo MapToTestInfo(
        this ServiceDescriptor source,
        ToDisplayNameOptions? options = null
    )
    {
        options ??= ToDisplayNameOptions.Default;

        return new ServiceDescriptorTestInfo(
            Lifetime: source.Lifetime,
            ServiceType: source.ServiceType.ToDisplayName(options),
            ServiceKey: source.IsKeyedService ? $"{source.ServiceKey}" : null,
            ImplementationType: source.IsKeyedService
                ? source.KeyedImplementationType?.ToDisplayName(options)
                : source.ImplementationType?.ToDisplayName(options),
            ImplementationInstance: source.IsKeyedService
                ? source.KeyedImplementationInstance?.GetType().ToDisplayName(options)
                : source.ImplementationInstance?.GetType().ToDisplayName(options),
            ImplementationFactory: source.IsKeyedService
                ? source.KeyedImplementationFactory?.Method.ToDisplayName(options)
                : source.ImplementationFactory?.Method.ToDisplayName(options)
        );
    }
}

public sealed record ServiceDescriptorTestInfo(
    ServiceLifetime Lifetime,
    string ServiceType,
    string? ServiceKey,
    string? ImplementationType = null,
    string? ImplementationInstance = null,
    string? ImplementationFactory = null
);

public readonly record struct CaptureRegisteredServicesTestResult(
    IReadOnlyCollection<ServiceDescriptorTestInfo> Added,
    IReadOnlyCollection<ServiceDescriptorTestInfo> Removed
);

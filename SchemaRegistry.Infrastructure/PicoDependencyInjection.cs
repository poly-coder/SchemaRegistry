using Microsoft.Extensions.DependencyInjection;
using Pico.Reflection;

namespace Pico.DependencyInjection;

public static class PicoDependencyInjectionExtensions
{
    public static CaptureRegisteredServicesResult CaptureRegisteredServices(
        this IServiceCollection services,
        Action<IServiceCollection> configure
    )
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        var before = services.ToHashSet();

        configure(services);

        var added = services.Where(d => !before.Contains(d)).ToArray();
        var removed = before.Except(services).ToArray();

        return new(Added: added, Removed: removed);
    }

    public static IServiceCollection LogRegisteredServices(
        this IServiceCollection services,
        string caption,
        Action<IServiceCollection> configure,
        TextWriter? output,
        ToDisplayNameOptions? options = null
    )
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        options ??= ToDisplayNameOptions.Default;

        if (output is null || output == TextWriter.Null)
        {
            configure(services);
            return services;
        }

        var (added, removed) = services.CaptureRegisteredServices(configure);

        var comparer = options.FullNames
            ? ServiceDescriptorComparer.FullNames
            : ServiceDescriptorComparer.Default;

        if (removed.Count > 0 || added.Count > 0)
        {
            output.Write(caption);
            output.Write(" -");
            output.Write(removed.Count);
            output.Write(" +");
            output.Write(added.Count);
            output.WriteLine();

            foreach (var descriptor in removed)
            {
                output.Write("  - ");
                output.WriteServiceDescriptor(descriptor, options);
                output.WriteLine();
            }

            foreach (var descriptor in added)
            {
                output.Write("  + ");
                output.WriteServiceDescriptor(descriptor, options);
                output.WriteLine();
            }
        }

        return services;
    }

    public static void WriteServiceDescriptor(
        this TextWriter output,
        ServiceDescriptor descriptor,
        ToDisplayNameOptions? options = null
    )
    {
        switch (descriptor.Lifetime)
        {
            case ServiceLifetime.Singleton:
                output.Write("Singleton ");
                break;
            case ServiceLifetime.Scoped:
                output.Write("Scoped ");
                break;
            case ServiceLifetime.Transient:
                output.Write("Transient ");
                break;
        }

        Delegate? factory;
        object? instance;
        Type? concrete;

        output.WriteType(descriptor.ServiceType, options);

        if (descriptor.IsKeyedService)
        {
            output.Write(" [");
            output.Write(descriptor.ServiceKey);
            output.Write("]");

            factory = descriptor.KeyedImplementationFactory;
            instance = descriptor.KeyedImplementationInstance;
            concrete = descriptor.KeyedImplementationType;
        }
        else
        {
            factory = descriptor.ImplementationFactory;
            instance = descriptor.ImplementationInstance;
            concrete = descriptor.ImplementationType;
        }

        output.Write(" => ");

        if (concrete is not null)
        {
            output.Write("type: ");
            output.WriteType(concrete, options);
        }
        else if (instance is not null)
        {
            output.Write("instance: ");
            output.WriteType(instance.GetType(), options);
        }
        else if (factory is not null)
        {
            output.Write("factory: ");
            output.WriteMethod(factory.Method, options);
        }
    }
}

public readonly record struct CaptureRegisteredServicesResult(
    IReadOnlyCollection<ServiceDescriptor> Added,
    IReadOnlyCollection<ServiceDescriptor> Removed
);

public sealed class ServiceDescriptorComparer(bool fullNames = false) : IComparer<ServiceDescriptor>
{
    public static readonly ServiceDescriptorComparer Default = new(false);
    public static readonly ServiceDescriptorComparer FullNames = new(true);

    public int Compare(ServiceDescriptor? x, ServiceDescriptor? y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }

        if (y is null)
        {
            return 1;
        }

        if (x is null)
        {
            return -1;
        }

        if (x.Lifetime.CompareTo(y.Lifetime) is var lifetimeComp && lifetimeComp != 0)
        {
            return lifetimeComp;
        }

        var xServiceType = fullNames ? x.ServiceType.FullName : x.ServiceType.Name;
        var yServiceType = fullNames ? y.ServiceType.FullName : y.ServiceType.Name;
        if (
            string.Compare(xServiceType, yServiceType, StringComparison.Ordinal) is var serviceComp
            && serviceComp != 0
        )
        {
            return serviceComp;
        }

        if (x.IsKeyedService)
        {
            if (y.IsKeyedService)
            {
                var xKey = $"{x.ServiceKey}";
                var yKey = $"{y.ServiceKey}";
                if (
                    string.Compare(xKey, yKey, StringComparison.Ordinal) is var keyComp
                    && keyComp != 0
                )
                {
                    return keyComp;
                }
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return 1;
        }

        return 0;
    }
}

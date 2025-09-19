using Pico.Reflection;

namespace Pico.DependencyInjection;

public static class PicoDependencyInjectionExtensions
{
    public static IServiceCollection LogRegisteredServices(
        this IServiceCollection services,
        string caption,
        Action<IServiceCollection> configure,
        TextWriter? output,
        bool fullNames = false
    )
    {
        if (output is null || output == TextWriter.Null)
        {
            configure(services);
            return services;
        }

        var before = services.ToArray();

        configure(services);

        var comparer = fullNames
            ? ServiceDescriptorComparer.FullNames
            : ServiceDescriptorComparer.Default;
        var removed = before.Except(services).Order(comparer).ToArray();
        var added = services.Except(before).Order(comparer).ToArray();

        if (removed.Length > 0 || added.Length > 0)
        {
            output.Write(caption);
            output.Write(" -");
            output.Write(removed.Length);
            output.Write(" +");
            output.Write(added.Length);
            output.WriteLine();

            foreach (var descriptor in removed)
            {
                output.Write("  - ");
                output.WriteServiceDescriptor(descriptor, fullNames);
                output.WriteLine();
            }

            foreach (var descriptor in added)
            {
                output.Write("  + ");
                output.WriteServiceDescriptor(descriptor, fullNames);
                output.WriteLine();
            }
        }

        return services;
    }

    public static void WriteServiceDescriptor(
        this TextWriter output,
        ServiceDescriptor descriptor,
        bool fullNames = false
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

        output.Write(descriptor.ServiceType.ToDisplayName(fullNames));

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
            output.Write(concrete.ToDisplayName(fullNames));
        }
        else if (instance is not null)
        {
            output.Write("instance: ");
            output.Write(instance.GetType().ToDisplayName(fullNames));
        }
        else if (factory is not null)
        {
            output.Write("factory: ");
            output.Write(factory.Method.DeclaringType?.ToDisplayName(fullNames));
            output.Write(".");
            output.Write(factory.Method.Name);
            output.Write("(...)");
        }
    }
}

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

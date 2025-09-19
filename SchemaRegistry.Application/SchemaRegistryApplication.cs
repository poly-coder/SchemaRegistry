using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace SchemaRegistry.Application;

public class SchemaRegistryApplication
{
    public static readonly Assembly Assembly = typeof(SchemaRegistryApplication).Assembly;

    public static readonly string Name = "SchemaRegistry.Application";

    public static readonly ActivitySource ActivitySource = new(Name);

    public static readonly Meter Meter = new(Name);
}

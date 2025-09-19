using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace SchemaRegistry.Infrastructure;

public class SchemaRegistryInfrastructure
{
    public static readonly Assembly Assembly = typeof(SchemaRegistryInfrastructure).Assembly;

    public static readonly string Name = "SchemaRegistry.Infrastructure";

    public static readonly ActivitySource ActivitySource = new(Name);

    public static readonly Meter Meter = new(Name);
}

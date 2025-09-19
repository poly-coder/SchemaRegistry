using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace SchemaRegistry.Domain;

public class SchemaRegistryDomain
{
    public static readonly Assembly Assembly = typeof(SchemaRegistryDomain).Assembly;

    public const string ProjectName = "SchemaRegistry";
    public const string Name = $"{ProjectName}.Domain";

    public static readonly ActivitySource ActivitySource = new(Name);

    public static readonly Meter Meter = new(Name);
}

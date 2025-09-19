using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using SchemaRegistry.Domain;

namespace SchemaRegistry.Application;

public class SchemaRegistryApplication
{
    public static readonly Assembly Assembly = typeof(SchemaRegistryApplication).Assembly;

    public const string Name = $"{SchemaRegistryDomain.ProjectName}.Application";

    public static readonly ActivitySource ActivitySource = new(Name);

    public static readonly Meter Meter = new(Name);
}

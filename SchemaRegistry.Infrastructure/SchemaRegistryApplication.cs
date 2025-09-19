using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using SchemaRegistry.Domain;

namespace SchemaRegistry.Infrastructure;

public class SchemaRegistryInfrastructure
{
    public static readonly Assembly Assembly = typeof(SchemaRegistryInfrastructure).Assembly;

    public const string Name = $"{SchemaRegistryDomain.ProjectName}.Infrastructure";

    public static readonly ActivitySource ActivitySource = new(Name);

    public static readonly Meter Meter = new(Name);
}

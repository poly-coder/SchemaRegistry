using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using SchemaRegistry.Domain;

namespace SchemaRegistry.WebApi;

public class SchemaRegistryWebApi
{
    public static readonly Assembly Assembly = typeof(SchemaRegistryWebApi).Assembly;

    public const string Name = $"{SchemaRegistryDomain.ProjectName}.WebApi";

    public static readonly ActivitySource ActivitySource = new(Name);

    public static readonly Meter Meter = new(Name);
}

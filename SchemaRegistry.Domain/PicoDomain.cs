using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace Pico.Domain;

public class PicoDomain
{
    public static readonly Assembly Assembly = typeof(PicoDomain).Assembly;

    public const string ProjectName = "Pico";
    public const string Name = $"{PicoDomain.ProjectName}.Domain";

    public static readonly ActivitySource ActivitySource = new(Name);

    public static readonly Meter Meter = new(Name);
}

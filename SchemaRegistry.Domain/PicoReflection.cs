using System.Collections.Frozen;

namespace Pico.Reflection;

public static class PicoReflectionExtensions
{
    private static readonly IReadOnlyDictionary<Type, string> KnownSimpleTypes = new Dictionary<
        Type,
        string
    >
    {
        { typeof(string), "string" },
        { typeof(byte), "byte" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(bool), "bool" },
        { typeof(char), "char" },
        { typeof(decimal), "decimal" },
        { typeof(DateTime), "DateTime" },
        { typeof(DateTimeOffset), "DateTimeOffset" },
        { typeof(TimeSpan), "TimeSpan" },
        { typeof(Guid), "Guid" },
    }.ToFrozenDictionary();

    public static string ToDisplayName(this Type type, bool fullNames = false)
    {
        if (KnownSimpleTypes.TryGetValue(type, out var name))
        {
            return name;
        }

        var nsPrefix = type.Namespace is { Length: > 0 } ns ? (fullNames ? ns + "." : "") : "";

        if (type.IsGenericType)
        {
            var typeName = type.Name.TakeBeforeFirst("`");
            var genericArgs = type.GetGenericArguments()
                .Select(t => t.ToDisplayName(fullNames))
                .ToArray();
            return $"{nsPrefix}{typeName}<{string.Join(", ", genericArgs)}>";
        }

        return fullNames ? (type.FullName ?? type.Name) : type.Name;
    }
}

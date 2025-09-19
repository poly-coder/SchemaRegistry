using System.Diagnostics.CodeAnalysis;

namespace Pico;

public static class PicoStringExtensions
{
    [return: NotNullIfNotNull(nameof(value))]
    public static string? CoerceTrim(this string? value) => value?.Trim();

    public static string TakeBeforeFirst(this string source, string value)
    {
        var index = source.IndexOf(value, StringComparison.Ordinal);
        return index < 0 ? source : source[..index];
    }
}

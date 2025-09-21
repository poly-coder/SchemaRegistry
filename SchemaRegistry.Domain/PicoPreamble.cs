using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pico;

public static class PicoStringExtensions
{
    #region [ Coerce ]

    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? CoerceTrim(this string? value) => value?.Trim();

    #endregion [ Coerce ]

    #region [ Take Parts ]

    public static string TakeBeforeIndexOrComplete(this string source, int index)
    {
        if (index < 0 || index >= source.Length)
            return source;
        return source[..index];
    }

    public static string TakeBeforeFirst(
        this string source,
        string value,
        StringComparison comparison = StringComparison.Ordinal
    ) => source.TakeBeforeIndexOrComplete(source.IndexOf(value, comparison));

    #endregion [ Take Parts ]
}

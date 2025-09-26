using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Pico;

public static class PicoStringExtensions
{
    #region [ Coerce ]

    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? CoerceTrim(this string? value) => value?.Trim();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string CoerceTrimRequired(this string? value) => value?.Trim() ?? "";

    public static IReadOnlyList<string?> CoerceTrim(this IReadOnlyList<string?> source)
    {
        int count = source.Count;
        var index = 0;
        var needsCoercion = false;

        for (; index < count; index++)
        {
            var item = source[index];
            var coercedItem = item.CoerceTrim();
            if (!ReferenceEquals(coercedItem, item))
            {
                needsCoercion = true;
                break;
            }
        }

        if (!needsCoercion)
            return source;

        var result = new string?[count];

        for (int i = 0; i < index; i++)
        {
            result[i] = source[i];
        }

        for (int i = index; i < count; i++)
        {
            result[i] = source[i].CoerceTrim();
        }

        return result;
    }

    public static IReadOnlyList<string> CoerceTrimRequired(this IReadOnlyList<string> source)
    {
        int count = source.Count;
        var index = 0;
        var needsCoercion = false;

        for (; index < count; index++)
        {
            var item = source[index];
            var coercedItem = item.CoerceTrimRequired();
            if (!ReferenceEquals(coercedItem, item))
            {
                needsCoercion = true;
                break;
            }
        }

        if (!needsCoercion)
            return source;

        var result = new string[count];

        for (int i = 0; i < index; i++)
        {
            result[i] = source[i];
        }

        for (int i = index; i < count; i++)
        {
            result[i] = source[i].CoerceTrimRequired();
        }

        return result;
    }

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

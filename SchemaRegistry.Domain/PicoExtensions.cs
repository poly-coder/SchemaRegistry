using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace Pico
{
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
}

namespace Pico.Reflection
{
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
}

namespace Pico.Domain.FluentValidation
{
    public static class PicoDomainValidationExtensions
    {
        public static async Task<T> ValidateWithAsync<T>(
            this T value,
            IValidator<T> validator,
            CancellationToken cancel = default
        )
        {
            await validator.ValidateAndThrowAsync(value, cancellationToken: cancel);

            return value;
        }

        public static T ValidateWith<T>(this T value, IValidator<T> validator)
        {
            validator.ValidateAndThrow(value);

            return value;
        }
    }
}

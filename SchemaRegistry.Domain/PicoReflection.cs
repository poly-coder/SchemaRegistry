using System.Collections.Frozen;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pico.Reflection;

public static class PicoReflectionExtensions
{
    #region [ Write / ToDisplayName ]

    private static readonly IReadOnlyDictionary<Type, string> KnownSimpleTypes = new Dictionary<
        Type,
        string
    >
    {
        { typeof(void), "void" },
        { typeof(object), "object" },
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
    }.ToFrozenDictionary();

    public static TextWriter WriteType(
        this TextWriter output,
        Type type,
        ToDisplayNameOptions? options = null,
        IEnumerable<CustomAttributeData>? attributes = null
    )
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(type);

        options ??= ToDisplayNameOptions.Default;

        bool isNullable = false;

        if (type.IsValueType && Nullable.GetUnderlyingType(type) is { } nullableSubType)
        {
            isNullable = true;
            type = nullableSubType;
        }

        if (KnownSimpleTypes.TryGetValue(type, out var name))
        {
            output.Write(name);
            if (isNullable)
                output.Write('?');
            return output;
        }

        if (type.Namespace is { Length: > 0 } ns && options.FullNames)
        {
            output.Write(ns);
            output.Write('.');
        }

        var typeName = type.Name.TakeBeforeFirst("`");

        if (typeName.Contains('<'))
        {
            typeName = "SpecialType";
        }

        output.Write(typeName);

        if (type.IsGenericType)
        {
            var genericArgs = type.GetGenericArguments();
            output.Write('<');
            for (int i = 0; i < genericArgs.Length; i++)
            {
                if (i > 0)
                    output.Write(", ");
                output.WriteType(genericArgs[i], options);
            }
            output.Write('>');
        }

        if (isNullable)
            output.Write('?');

        return output;
    }

    public static string ToDisplayName(this Type type, ToDisplayNameOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(type);

        var sb = new StringBuilder();
        using var output = new StringWriter(sb);

        output.WriteType(type, options);

        output.Flush();

        return sb.ToString();
    }

    public static TextWriter WriteMethod(
        this TextWriter output,
        MethodInfo method,
        ToDisplayNameOptions? options = null
    )
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(method);

        options ??= ToDisplayNameOptions.Default;

        if (method.IsStatic)
        {
            output.Write("static ");
        }

        output.WriteType(method.ReturnType, options, method.ReturnParameter.CustomAttributes);
        output.Write(' ');

        if (method.DeclaringType is { } declaringType)
        {
            output.WriteType(declaringType, options);
            output.Write('.');
        }

        var methodName = method.Name;

        if (methodName.Contains('<'))
        {
            methodName = "SpecialMethod";
        }

        output.Write(methodName);

        if (method.IsGenericMethod)
        {
            var arguments = method.GetGenericArguments();
            output.Write('<');
            for (int i = 0; i < arguments.Length; i++)
            {
                if (i > 0)
                    output.Write(", ");
                output.WriteType(arguments[i], options);
            }
            output.Write('>');
        }

        output.Write('(');

        var parameters = method.GetParameters();
        for (int i = 0; i < parameters.Length; i++)
        {
            if (i > 0)
                output.Write(", ");
            var parameter = parameters[i];
            output.WriteType(parameter.ParameterType, options, parameter.CustomAttributes);
            output.Write(' ');
            output.Write(parameter.Name);
        }

        output.Write(')');

        return output;
    }

    public static string ToDisplayName(this MethodInfo method, ToDisplayNameOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(method);

        var sb = new StringBuilder();
        using var output = new StringWriter(sb);

        output.WriteMethod(method, options);

        output.Flush();

        return sb.ToString();
    }

    #endregion [ Write / ToDisplayName ]

    #region [ Extract ]

    public static MethodInfo ExtractMethod(this Expression expression)
    {
        return expression switch
        {
            MethodCallExpression callExpr => callExpr.Method,
            LambdaExpression lambdaExpr => lambdaExpr.Body.ExtractMethod(),
            BinaryExpression { Method: { } method } => method,

            _ => throw new ArgumentException("Expression is not a method call", nameof(expression)),
        };
    }

    public static PropertyInfo ExtractProperty(this Expression expression)
    {
        return expression switch
        {
            MemberExpression { Member: PropertyInfo prop } => prop,
            LambdaExpression lambdaExpr => lambdaExpr.Body.ExtractProperty(),

            _ => throw new ArgumentException(
                "Expression is not a property access",
                nameof(expression)
            ),
        };
    }

    #endregion [ Extract ]
}

public sealed record ToDisplayNameOptions(bool FullNames = false)
{
    public static readonly ToDisplayNameOptions Default = new();
}

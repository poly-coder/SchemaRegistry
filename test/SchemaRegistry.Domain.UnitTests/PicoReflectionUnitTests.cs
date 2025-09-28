using System.Collections;
using System.Reflection;
using Pico.Testing;

namespace Pico.Reflection.UnitTests;

[UnitTest]
public class PicoReflectionExtensionsUnitTests
{
    #region [ Write / ToDisplayName ]

    // WriteType

    [Theory]
    [MemberData(nameof(GetWriteTypeTestData))]
    public void WriteTypeTest(WriteTypeTestData test)
    {
        var actual = test.Type.ToDisplayName(test.Options);

        actual.ShouldBe(test.Expected);
    }

    public sealed record WriteTypeTestData(
        Type Type,
        string Expected,
        ToDisplayNameOptions? Options = null
    );

    public static TheoryData<WriteTypeTestData> GetWriteTypeTestData() =>
        [
            // Built-in types
            new(typeof(void), "void"),
            new(typeof(object), "object"),
            new(typeof(string), "string"),
            new(typeof(byte), "byte"),
            new(typeof(sbyte), "sbyte"),
            new(typeof(short), "short"),
            new(typeof(ushort), "ushort"),
            new(typeof(int), "int"),
            new(typeof(uint), "uint"),
            new(typeof(long), "long"),
            new(typeof(ulong), "ulong"),
            new(typeof(float), "float"),
            new(typeof(double), "double"),
            new(typeof(bool), "bool"),
            new(typeof(char), "char"),
            new(typeof(decimal), "decimal"),
            // Non-built-in types
            new(typeof(DateTime), "DateTime"),
            new(typeof(DateTime), "System.DateTime", new(FullNames: true)),
            // Generic types
            new(typeof(IEnumerable<DateTime>), "IEnumerable<DateTime>"),
            new(
                typeof(IEnumerable<DateTime>),
                "System.Collections.Generic.IEnumerable<System.DateTime>",
                new(FullNames: true)
            ),
            new(typeof(IEnumerable<>), "IEnumerable<T>"),
            new(typeof(IDictionary<string, DateTime>), "IDictionary<string, DateTime>"),
            new(
                typeof(IDictionary<string, DateTime>),
                "System.Collections.Generic.IDictionary<string, System.DateTime>",
                new(FullNames: true)
            ),
            new(typeof(IDictionary<,>), "IDictionary<TKey, TValue>"),
            // Nullable types
            new(typeof(int?), "int?"),
            new(typeof(DateTime?), "DateTime?"),
        ];

    // WriteMethod

    [Theory]
    [MemberData(nameof(GetWriteMethodTestData))]
    public void WriteMethodTest(WriteMethodTestData test)
    {
        var actual = test.Method.ToDisplayName(test.Options);

        actual.ShouldBe(test.Expected);
    }

    public sealed record WriteMethodTestData(
        MethodInfo Method,
        string Expected,
        ToDisplayNameOptions? Options = null
    );

    public static TheoryData<WriteMethodTestData> GetWriteMethodTestData()
    {
        var lambda1 = () => { };
        var lambda2 = (IEnumerable<string> e, int c) => 3.1415M;

        return
        [
            // Simple methods
            new(
                PicoReflectionExtensions.ExtractMethod((object o) => o.ToString()),
                "string object.ToString()"
            ),
            new(
                PicoReflectionExtensions.ExtractMethod((object o) => o.Equals(null)),
                "bool object.Equals(object obj)"
            ),
            // Generic types methods
            new(
                PicoReflectionExtensions.ExtractMethod(
                    (IEnumerable<string> e) => e.GetEnumerator()
                ),
                "IEnumerator<string> IEnumerable<string>.GetEnumerator()"
            ),
            new(
                PicoReflectionExtensions.ExtractMethod(
                    (Dictionary<string, int> e) => e.Add("key", 1)
                ),
                "void Dictionary<string, int>.Add(string key, int value)"
            ),
            // Generic methods
            new(
                PicoReflectionExtensions.ExtractMethod((IEnumerable e) => e.Cast<string>()),
                "static IEnumerable<string> Enumerable.Cast<string>(IEnumerable source)"
            ),
            new(
                PicoReflectionExtensions.ExtractMethod(
                    (IEnumerable<DateTime> e) =>
                        e.ToDictionary(x => x.Year, x => x.ToLongDateString())
                ),
                "static Dictionary<int, string> Enumerable.ToDictionary<DateTime, int, string>(IEnumerable<DateTime> source, Func<DateTime, int> keySelector, Func<DateTime, string> elementSelector)"
            ),
            // Lambda methods
            new(lambda1.Method, "void SpecialType.SpecialMethod()"),
            new(
                lambda1.Method,
                "void Pico.Reflection.UnitTests.SpecialType.SpecialMethod()",
                new ToDisplayNameOptions(FullNames: true)
            ),
            new(lambda2.Method, "decimal SpecialType.SpecialMethod(IEnumerable<string> e, int c)"),
            new(
                lambda2.Method,
                "decimal Pico.Reflection.UnitTests.SpecialType.SpecialMethod(System.Collections.Generic.IEnumerable<string> e, int c)",
                new ToDisplayNameOptions(FullNames: true)
            ),
        ];
    }

    #endregion [ Write / ToDisplayName ]
}

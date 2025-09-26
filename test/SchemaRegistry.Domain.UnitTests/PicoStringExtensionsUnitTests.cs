using Pico.Testing;

namespace Pico.UnitTests;

[UnitTest]
public class PicoStringExtensionsUnitTests
{
    #region [ Coerce ]

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData(" \r\n\t\f ", "")]
    [InlineData("Display name", "Display name")]
    [InlineData(" \r\n\t\f Display name  \r\n\t\f ", "Display name")]
    public void CoerceTrimTest(string? value, string? expected)
    {
        var actual = value.CoerceTrim();
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData(" \r\n\t\f ", "")]
    [InlineData("Display name", "Display name")]
    [InlineData(" \r\n\t\f Display name  \r\n\t\f ", "Display name")]
    public void CoerceTrimRequiredTest(string? value, string? expected)
    {
        var actual = value.CoerceTrimRequired();
        actual.ShouldBe(expected);
    }

    [Theory]
    [MemberData(nameof(GetListCoerceTrimTestData))]
    public void ListCoerceTrimTest(ListCoerceTrimTestData test)
    {
        var actual = test.Values.CoerceTrim();
        actual.ShouldBe(test.Expected);
    }

    public sealed record ListCoerceTrimTestData(string?[] Values, string?[] Expected);

    public static TheoryData<ListCoerceTrimTestData> GetListCoerceTrimTestData() =>
        [
            new([], []),
            new([null], [null]),
            new([" \r\n\t\f"], [""]),
            new(["coerced"], ["coerced"]),
            new(["  spaces  "], ["spaces"]),
            new([null, " ", "hello", " world "], [null, "", "hello", "world"]),
        ];

    [Theory]
    [MemberData(nameof(GetListCoerceTrimRequiredTestData))]
    public void ListCoerceTrimRequiredTest(ListCoerceTrimRequiredTestData test)
    {
        var actual = test.Values.CoerceTrimRequired();
        actual.ShouldBe(test.Expected);
    }

    public sealed record ListCoerceTrimRequiredTestData(string[] Values, string[] Expected);

    public static TheoryData<ListCoerceTrimRequiredTestData> GetListCoerceTrimRequiredTestData() =>
        [
            new([], []),
            new([null!], [""]),
            new([" \r\n\t\f"], [""]),
            new(["coerced"], ["coerced"]),
            new(["  spaces  "], ["spaces"]),
            new([null!, " ", "hello", " world "], ["", "", "hello", "world"]),
        ];

    #endregion [ Coerce ]

    #region [ Take Parts ]

    [Theory]
    [InlineData("Display name", " n", "Display")]
    [InlineData("Display name", "x", "Display name")]
    public void TakeBeforeFirstTest(string source, string value, string expected)
    {
        var actual = source.TakeBeforeFirst(value);
        actual.ShouldBe(expected);
    }

    #endregion [ Take Parts ]
}

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

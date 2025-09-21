using Pico.Testing;

namespace Pico.Domain.UnitTests;

[UnitTest]
public class PicoDomainUnitTests
{
    [Fact]
    public void ConfigTest()
    {
        PicoDomain.Assembly.ShouldSatisfyAllConditions(
            () => PicoDomain.Assembly.ShouldBe(typeof(PicoDomain).Assembly),
            () => PicoDomain.ProjectName.ShouldBe("Pico"),
            () => PicoDomain.Name.ShouldBe("Pico.Domain"),
            () => PicoDomain.ActivitySource.Name.ShouldBe("Pico.Domain"),
            () => PicoDomain.Meter.Name.ShouldBe("Pico.Domain")
        );
    }
}

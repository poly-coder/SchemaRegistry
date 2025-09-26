using Pico.Testing;

namespace SchemaRegistry.Application.UnitTests;

[UnitTest]
public class SchemaRegistryApplicationUnitTests
{
    [Fact]
    public void ConfigTest()
    {
        SchemaRegistryApplication.Assembly.ShouldSatisfyAllConditions(
            () =>
                SchemaRegistryApplication.Assembly.ShouldBe(
                    typeof(SchemaRegistryApplication).Assembly
                ),
            () => SchemaRegistryApplication.Name.ShouldBe("SchemaRegistry.Application"),
            () =>
                SchemaRegistryApplication.ActivitySource.Name.ShouldBe(
                    "SchemaRegistry.Application"
                ),
            () => SchemaRegistryApplication.Meter.Name.ShouldBe("SchemaRegistry.Application")
        );
    }
}

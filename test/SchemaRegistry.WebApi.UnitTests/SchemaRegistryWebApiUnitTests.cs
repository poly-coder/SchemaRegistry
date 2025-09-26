using Pico.Testing;

namespace SchemaRegistry.WebApi.UnitTests;

[UnitTest]
public class SchemaRegistryWebApiUnitTests
{
    [Fact]
    public void ConfigTest()
    {
        SchemaRegistryWebApi.Assembly.ShouldSatisfyAllConditions(
            () => SchemaRegistryWebApi.Assembly.ShouldBe(typeof(SchemaRegistryWebApi).Assembly),
            () => SchemaRegistryWebApi.Name.ShouldBe("SchemaRegistry.WebApi"),
            () => SchemaRegistryWebApi.ActivitySource.Name.ShouldBe("SchemaRegistry.WebApi"),
            () => SchemaRegistryWebApi.Meter.Name.ShouldBe("SchemaRegistry.WebApi")
        );
    }
}

using Pico.Testing;

namespace SchemaRegistry.Infrastructure.UnitTests;

[UnitTest]
public class SchemaRegistryInfrastructureUnitTests
{
    [Fact]
    public void ConfigTest()
    {
        SchemaRegistryInfrastructure.Assembly.ShouldSatisfyAllConditions(
            () =>
                SchemaRegistryInfrastructure.Assembly.ShouldBe(
                    typeof(SchemaRegistryInfrastructure).Assembly
                ),
            () => SchemaRegistryInfrastructure.Name.ShouldBe("SchemaRegistry.Infrastructure"),
            () =>
                SchemaRegistryInfrastructure.ActivitySource.Name.ShouldBe(
                    "SchemaRegistry.Infrastructure"
                ),
            () => SchemaRegistryInfrastructure.Meter.Name.ShouldBe("SchemaRegistry.Infrastructure")
        );
    }
}

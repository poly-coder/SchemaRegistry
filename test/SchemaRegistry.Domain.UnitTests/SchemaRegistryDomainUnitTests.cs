using Pico.Testing;

namespace SchemaRegistry.Domain.UnitTests;

[UnitTest]
public class SchemaRegistryDomainUnitTests
{
    [Fact]
    public void ConfigTest()
    {
        SchemaRegistryDomain.Assembly.ShouldSatisfyAllConditions(
            () => SchemaRegistryDomain.Assembly.ShouldBe(typeof(SchemaRegistryDomain).Assembly),
            () => SchemaRegistryDomain.ProjectName.ShouldBe("SchemaRegistry"),
            () => SchemaRegistryDomain.Name.ShouldBe("SchemaRegistry.Domain"),
            () => SchemaRegistryDomain.ActivitySource.Name.ShouldBe("SchemaRegistry.Domain"),
            () => SchemaRegistryDomain.Meter.Name.ShouldBe("SchemaRegistry.Domain")
        );
    }
}

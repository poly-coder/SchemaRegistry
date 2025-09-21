using Pico.Testing;

namespace Pico.Domain.Errors.UnitTests;

[UnitTest]
public class PicoDomainErrorsUnitTests
{
    [Fact]
    public void AlreadyExistsExceptionTest()
    {
        var exception = new AlreadyExistsException("my-type", "id-1234");

        exception.ShouldSatisfyAllConditions(
            () => exception.EntityType.ShouldBe("my-type"),
            () => exception.EntityId.ShouldBe("id-1234"),
            () => exception.Message.ShouldContain("already exists"),
            () => exception.Message.ShouldContain("my-type"),
            () => exception.Message.ShouldContain("id-1234")
        );
    }
}

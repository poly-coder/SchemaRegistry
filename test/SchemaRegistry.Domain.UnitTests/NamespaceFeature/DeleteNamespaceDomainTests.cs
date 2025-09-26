using Pico.Testing;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Domain.UnitTests.NamespaceFeature;

[UnitTest]
public class DeleteNamespaceCommandValidatorTests
{
    [Theory]
    [MemberData(nameof(GetValidateTestData))]
    public async Task ValidateTest(ValidateTestData test)
    {
        var validator = new DeleteNamespaceCommandValidator();

        var result = await validator.ValidateAsync(test.Input);

        await Verify(new { result }).UseSnapshotsDirectory().UseCaseName(test.Name);
    }

    public sealed record ValidateTestData(string Name, DeleteNamespaceCommand Input);

    public static TheoryData<ValidateTestData> GetValidateTestData() =>
        [
            new("Null", new DeleteNamespaceCommand(Name: "")),
            new("Valid", new DeleteNamespaceCommand(Name: "my-namespace")),
            new("Invalid", new DeleteNamespaceCommand(Name: "invalid namespace")),
        ];
}

[UnitTest]
public class DeleteNamespaceCommandExtensionsTests
{
    [Theory]
    [MemberData(nameof(GetCoerceTestData))]
    public void CoerceTest(CoerceTestData test)
    {
        var actual = test.Input.Coerce();

        actual.ShouldBe(test.Expected);
    }

    public sealed record CoerceTestData(
        string Name,
        DeleteNamespaceCommand Input,
        DeleteNamespaceCommand Expected
    );

    public static TheoryData<CoerceTestData> GetCoerceTestData() =>
        [
            new(
                "Null",
                new DeleteNamespaceCommand(Name: null!),
                new DeleteNamespaceCommand(Name: "")
            ),
            new(
                "Clean",
                new DeleteNamespaceCommand(Name: "my-namespace"),
                new DeleteNamespaceCommand(Name: "my-namespace")
            ),
            new(
                "Coercing",
                new DeleteNamespaceCommand(Name: " \t\r\n\fmy-namespace \t\r\n\f"),
                new DeleteNamespaceCommand(Name: "my-namespace")
            ),
        ];
}

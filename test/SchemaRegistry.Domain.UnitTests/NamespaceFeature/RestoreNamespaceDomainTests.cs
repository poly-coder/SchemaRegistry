using Pico.Testing;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Domain.UnitTests.NamespaceFeature;

[UnitTest]
public class RestoreNamespaceCommandValidatorTests
{
    [Theory]
    [MemberData(nameof(GetValidateTestData))]
    public async Task ValidateTest(ValidateTestData test)
    {
        var validator = new RestoreNamespaceCommandValidator();

        var result = await validator.ValidateAsync(test.Input);

        await Verify(new { result }).UseSnapshotsDirectory().UseCaseName(test.Name);
    }

    public sealed record ValidateTestData(string Name, RestoreNamespaceCommand Input);

    public static TheoryData<ValidateTestData> GetValidateTestData() =>
        [
            new("Null", new RestoreNamespaceCommand(Name: "")),
            new("Valid", new RestoreNamespaceCommand(Name: "my-namespace")),
            new("Invalid", new RestoreNamespaceCommand(Name: "invalid namespace")),
        ];
}

[UnitTest]
public class RestoreNamespaceCommandExtensionsTests
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
        RestoreNamespaceCommand Input,
        RestoreNamespaceCommand Expected
    );

    public static TheoryData<CoerceTestData> GetCoerceTestData() =>
        [
            new(
                "Null",
                new RestoreNamespaceCommand(Name: null!),
                new RestoreNamespaceCommand(Name: "")
            ),
            new(
                "Clean",
                new RestoreNamespaceCommand(Name: "my-namespace"),
                new RestoreNamespaceCommand(Name: "my-namespace")
            ),
            new(
                "Coercing",
                new RestoreNamespaceCommand(Name: " \t\r\n\fmy-namespace \t\r\n\f"),
                new RestoreNamespaceCommand(Name: "my-namespace")
            ),
        ];
}

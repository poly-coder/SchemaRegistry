using Pico.Testing;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Domain.UnitTests.NamespaceFeature;

[UnitTest]
public class UpdateNamespaceDocumentationCommandValidatorTests
{
    [Theory]
    [MemberData(nameof(GetValidateTestData))]
    public async Task ValidateTest(ValidateTestData test)
    {
        var validator = new UpdateNamespaceDocumentationCommandValidator();

        var result = await validator.ValidateAsync(test.Input);

        await Verify(new { result }).UseSnapshotsDirectory().UseCaseName(test.Name);
    }

    public sealed record ValidateTestData(string Name, UpdateNamespaceDocumentationCommand Input);

    public static TheoryData<ValidateTestData> GetValidateTestData() =>
        [
            new("Null", new UpdateNamespaceDocumentationCommand(Name: null!, Documentation: null)),
            new(
                "Valid",
                new UpdateNamespaceDocumentationCommand(
                    Name: "my-namespace",
                    Documentation: "# Simple Documentation"
                )
            ),
            new(
                "Invalid",
                new UpdateNamespaceDocumentationCommand(
                    Name: "invalid namespace",
                    Documentation: "It is hard to create an invalid documentation"
                )
            ),
        ];
}

[UnitTest]
public class UpdateNamespaceDocumentationCommandExtensionsTests
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
        UpdateNamespaceDocumentationCommand Input,
        UpdateNamespaceDocumentationCommand Expected
    );

    public static TheoryData<CoerceTestData> GetCoerceTestData() =>
        [
            new(
                "Null",
                new UpdateNamespaceDocumentationCommand(Name: null!, Documentation: null),
                new UpdateNamespaceDocumentationCommand(Name: "", Documentation: null)
            ),
            new(
                "Clean",
                new UpdateNamespaceDocumentationCommand(
                    Name: "my-namespace",
                    Documentation: "# Simple Documentation"
                ),
                new UpdateNamespaceDocumentationCommand(
                    Name: "my-namespace",
                    Documentation: "# Simple Documentation"
                )
            ),
            new(
                "Coercing",
                new UpdateNamespaceDocumentationCommand(
                    Name: " \t\r\n\fmy-namespace \t\r\n\f",
                    Documentation: "# Simple Documentation\r\n"
                ),
                new UpdateNamespaceDocumentationCommand(
                    Name: "my-namespace",
                    Documentation: "# Simple Documentation\r\n"
                )
            ),
        ];
}

using Pico.Testing;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Domain.UnitTests.NamespaceFeature;

[UnitTest]
public class UpdateNamespaceDescriptionsCommandValidatorTests
{
    [Theory]
    [MemberData(nameof(GetValidateTestData))]
    public async Task ValidateTest(ValidateTestData test)
    {
        var validator = new UpdateNamespaceDescriptionsCommandValidator();

        var result = await validator.ValidateAsync(test.Input);

        await Verify(new { result }).UseSnapshotsDirectory().UseCaseName(test.Name);
    }

    public sealed record ValidateTestData(string Name, UpdateNamespaceDescriptionsCommand Input);

    public static TheoryData<ValidateTestData> GetValidateTestData() =>
        [
            new(
                "Null",
                new UpdateNamespaceDescriptionsCommand(
                    Name: null!,
                    DisplayName: null,
                    Description: null
                )
            ),
            new(
                "Valid",
                new UpdateNamespaceDescriptionsCommand(
                    Name: "my-namespace",
                    DisplayName: "My Namespace",
                    Description: "My Namespace Description"
                )
            ),
            new(
                "Invalid",
                new UpdateNamespaceDescriptionsCommand(
                    Name: "invalid namespace",
                    DisplayName: "  Do not use leading spaces",
                    Description: "Do not use trailing spaces  "
                )
            ),
        ];
}

[UnitTest]
public class UpdateNamespaceDescriptionsCommandExtensionsTests
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
        UpdateNamespaceDescriptionsCommand Input,
        UpdateNamespaceDescriptionsCommand Expected
    );

    public static TheoryData<CoerceTestData> GetCoerceTestData() =>
        [
            new(
                "Null",
                new UpdateNamespaceDescriptionsCommand(
                    Name: null!,
                    DisplayName: null,
                    Description: null
                ),
                new UpdateNamespaceDescriptionsCommand(
                    Name: "",
                    DisplayName: null,
                    Description: null
                )
            ),
            new(
                "Clean",
                new UpdateNamespaceDescriptionsCommand(
                    Name: "my-namespace",
                    DisplayName: "My Namespace",
                    Description: "My Namespace Description"
                ),
                new UpdateNamespaceDescriptionsCommand(
                    Name: "my-namespace",
                    DisplayName: "My Namespace",
                    Description: "My Namespace Description"
                )
            ),
            new(
                "Coercing",
                new UpdateNamespaceDescriptionsCommand(
                    Name: " \t\r\n\fmy-namespace \t\r\n\f",
                    DisplayName: " \t\r\n\fMy Namespace \t\r\n\f",
                    Description: " \t\r\n\fMy Namespace Description \t\r\n\f"
                ),
                new UpdateNamespaceDescriptionsCommand(
                    Name: "my-namespace",
                    DisplayName: "My Namespace",
                    Description: "My Namespace Description"
                )
            ),
        ];
}

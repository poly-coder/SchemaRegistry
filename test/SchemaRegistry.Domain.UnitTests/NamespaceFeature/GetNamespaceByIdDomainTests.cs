using Pico.Testing;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Domain.UnitTests.NamespaceFeature;

[UnitTest]
public class GetNamespaceByIdQueryValidatorTests
{
    [Theory]
    [MemberData(nameof(GetValidateTestData))]
    public async Task ValidateTest(ValidateTestData test)
    {
        var validator = new GetNamespaceByIdQueryValidator();

        var result = await validator.ValidateAsync(test.Input);

        await Verify(new { result }).UseSnapshotsDirectory().UseCaseName(test.Name);
    }

    public sealed record ValidateTestData(string Name, GetNamespaceByIdQuery Input);

    public static TheoryData<ValidateTestData> GetValidateTestData() =>
        [
            new("Null", new GetNamespaceByIdQuery(Name: "", Deleted: true)),
            new("Valid", new GetNamespaceByIdQuery(Name: "my-namespace", Deleted: false)),
            new("Invalid", new GetNamespaceByIdQuery(Name: "invalid namespace", Deleted: false)),
        ];
}

[UnitTest]
public class GetNamespaceByIdQueryExtensionsTests
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
        GetNamespaceByIdQuery Input,
        GetNamespaceByIdQuery Expected
    );

    public static TheoryData<CoerceTestData> GetCoerceTestData() =>
        [
            new(
                "Null",
                new GetNamespaceByIdQuery(Name: null!, Deleted: true),
                new GetNamespaceByIdQuery(Name: "", Deleted: true)
            ),
            new(
                "Clean",
                new GetNamespaceByIdQuery(Name: "my-namespace", Deleted: false),
                new GetNamespaceByIdQuery(Name: "my-namespace", Deleted: false)
            ),
            new(
                "Coercing",
                new GetNamespaceByIdQuery(Name: " \t\r\n\fmy-namespace \t\r\n\f", Deleted: false),
                new GetNamespaceByIdQuery(Name: "my-namespace", Deleted: false)
            ),
        ];
}

using Pico.Testing;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Domain.UnitTests.NamespaceFeature;

[UnitTest]
public class GetNamespaceByIdsQueryTests
{
    [Theory]
    [MemberData(nameof(GetEqualityTestData))]
    public void EqualityTest(EqualityTestData test)
    {
        test.ShouldSatisfyAllConditions(
            () => test.Left.Equals(test.Right).ShouldBe(test.ExpectedEquals),
            () =>
                test
                    .Left.GetHashCode()
                    .Equals(test.Right?.GetHashCode())
                    .ShouldBe(test.ExpectedEquals)
        );
    }

    public sealed record EqualityTestData(
        string Name,
        GetNamespaceByIdsQuery Left,
        GetNamespaceByIdsQuery? Right,
        bool ExpectedEquals
    );

    public static TheoryData<EqualityTestData> GetEqualityTestData()
    {
        var a = new GetNamespaceByIdsQuery(Names: ["some", "test"], Deleted: true);
        var b = new GetNamespaceByIdsQuery(Names: ["some", "test"], Deleted: true);
        var c = new GetNamespaceByIdsQuery(Names: ["other", "test"], Deleted: false);

        return
        [
            new("Same", a, a, true),
            new("Equals", a, b, true),
            new("Null", a, null, false),
            new("Distinct", a, c, false),
        ];
    }

    [Theory]
    [MemberData(nameof(GetToStringTestData))]
    public void ToStringTest(ToStringTestData test)
    {
        test.Input.ToString().ShouldBe(test.Expected);
    }

    public sealed record ToStringTestData(GetNamespaceByIdsQuery Input, string Expected);

    public static TheoryData<ToStringTestData> GetToStringTestData()
    {
        return
        [
            new(
                new(Names: [], Deleted: true),
                "GetNamespaceByIdsQuery { Names = [], Deleted = True }"
            ),
            new(
                new(Names: ["some", "test"], Deleted: false),
                "GetNamespaceByIdsQuery { Names = [some, test], Deleted = False }"
            ),
        ];
    }
}

[UnitTest]
public class GetNamespaceByIdsQueryValidatorTests
{
    [Theory]
    [MemberData(nameof(GetValidateTestData))]
    public async Task ValidateTest(ValidateTestData test)
    {
        var validator = new GetNamespaceByIdsQueryValidator();

        var result = await validator.ValidateAsync(test.Input);

        await Verify(new { result }).UseSnapshotsDirectory().UseCaseName(test.Name);
    }

    public sealed record ValidateTestData(string Name, GetNamespaceByIdsQuery Input);

    public static TheoryData<ValidateTestData> GetValidateTestData() =>
        [
            new("Null", new GetNamespaceByIdsQuery(Names: [null!], Deleted: true)),
            new("Valid", new GetNamespaceByIdsQuery(Names: ["my-namespace"], Deleted: false)),
            new(
                "Invalid",
                new GetNamespaceByIdsQuery(Names: ["invalid namespace"], Deleted: false)
            ),
        ];
}

[UnitTest]
public class GetNamespaceByIdsQueryExtensionsTests
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
        GetNamespaceByIdsQuery Input,
        GetNamespaceByIdsQuery Expected
    );

    public static TheoryData<CoerceTestData> GetCoerceTestData() =>
        [
            new(
                "Empty",
                new GetNamespaceByIdsQuery(Names: [], Deleted: true),
                new GetNamespaceByIdsQuery(Names: [], Deleted: true)
            ),
            new(
                "Clean",
                new GetNamespaceByIdsQuery(Names: ["my-namespace"], Deleted: false),
                new GetNamespaceByIdsQuery(Names: ["my-namespace"], Deleted: false)
            ),
            new(
                "Coercing",
                new GetNamespaceByIdsQuery(
                    Names: [" \t\r\n\fmy-namespace \t\r\n\f", null!],
                    Deleted: false
                ),
                new GetNamespaceByIdsQuery(Names: ["my-namespace", ""], Deleted: false)
            ),
        ];
}

using FluentValidation;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Domain.UnitTests.NamespaceFeature;

public sealed class NamespaceValidationsTests
{
    /// <summary>
    /// Test the following Functional Requirements for Namespace names:
    /// - **FR-016**: System MUST validate namespace names using pattern "my-namespace123" (lowercase letters, numbers, hyphens) with maximum 40 characters
    /// </summary>
    /// <param name="name"></param>
    /// <param name="expectedOk"></param>
    /// <param name="errorSnippet"></param>
    [Theory]
    [InlineData("core", true, null)]
    [InlineData("user-service", true, null)]
    [InlineData("payment-v2", true, null)]
    [InlineData("analytics123", true, null)]
    [InlineData("User-Service", false, "lowercase")]
    [InlineData("-payment", false, "hyphens")]
    [InlineData("payment-", false, "hyphens")]
    [InlineData("payment--v2", false, "hyphens")]
    [InlineData("123core", false, "simple domain name")]
    [InlineData("    ", false, "empty")]
    [InlineData("", false, "empty")]
    [InlineData(null, false, "empty")]
    public void IsValidNamespaceNameTest(string? name, bool expectedOk, string? errorSnippet)
    {
        var validator = new ClassWithNameValidator();

        var input = new ClassWithName(name!);

        var result = validator.Validate(input);

        var nameErrors = result
            .Errors.Where(e => e.PropertyName == nameof(ClassWithName.Name))
            .ToArray();

        if (expectedOk)
        {
            nameErrors.ShouldBeEmpty();
        }
        else
        {
            nameErrors.ShouldSatisfyAllConditions(
                () => nameErrors.ShouldNotBeEmpty(),
                () => nameErrors.ShouldContain(e => e.ErrorMessage.Contains(errorSnippet!))
            );
        }
    }

    public sealed record ClassWithName(string Name);

    public sealed class ClassWithNameValidator : AbstractValidator<ClassWithName>
    {
        public ClassWithNameValidator()
        {
            RuleFor(x => x.Name).IsValidNamespaceName();
        }
    }
}

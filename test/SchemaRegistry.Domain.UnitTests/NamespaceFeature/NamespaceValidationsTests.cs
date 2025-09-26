using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentValidation;
using Pico.Testing;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Domain.UnitTests.NamespaceFeature;

[UnitTest]
public sealed class NamespaceValidationsTests
{
    #region [ Name ]

    /// <summary>
    /// Test the following Functional Requirements for Namespace names:
    /// - **FR-016**: System MUST validate namespace names using pattern "my-namespace123" (lowercase letters, numbers, hyphens) with maximum 40 characters
    /// </summary>
    [Theory]
    [MemberData(nameof(GetNameValidationTestData))]
    public async Task NameValidationTest(NameValidationTestData test)
    {
        var validator = new ClassWithNameValidator();

        var input = new ClassWithName(test.Name!);

        var result = await validator.ValidateAsync(input);

        var nameErrors = result
            .Errors.Where(e => e.PropertyName == nameof(ClassWithName.Name))
            .ToArray();

        if (test.ExpectedOk)
        {
            nameErrors.ShouldBeEmpty();
        }
        else
        {
            nameErrors.ShouldSatisfyAllConditions(
                () => nameErrors.ShouldNotBeEmpty(),
                () => nameErrors.ShouldContain(e => e.ErrorMessage.Contains(test.ErrorSnippet!))
            );
        }
    }

    public sealed record NameValidationTestData(
        string? Name,
        bool ExpectedOk = true,
        [property: MemberNotNullWhen(false, nameof(NameValidationTestData.ExpectedOk))]
            string? ErrorSnippet = null
    );

    public static TheoryData<NameValidationTestData> GetNameValidationTestData()
    {
        return
        [
            // Valid names
            new("core"),
            new("user-service"),
            new("payment-v2"),
            new("analytics123"),
            new("ns" + GenerateDigits(38)),
            // Names with Uppercase letters
            new("User-Service", false, "lowercase"),
            new("NAMESPACE", false, "lowercase"),
            // Names with invalid hyphen usage
            new("-payment", false, "hyphens"),
            new("payment-", false, "hyphens"),
            new("payment--v2", false, "hyphens"),
            // Names with starting digits
            new("123", false, "numbers"),
            new("123core", false, "numbers"),
            // Names that are too long
            new("ns" + GenerateDigits(39), false, "length"),
            // Names with invalid characters
            new("namespace_with_underscore", false, "simple domain name"),
            new("namespace.with.dot", false, "simple domain name"),
            new("namespace with space", false, "simple domain name"),
            new("namespace$special", false, "simple domain name"),
            // Empty or whitespace names
            new("    ", false, "empty"),
            new("", false, "empty"),
            new(null, false, "empty"),
        ];
    }

    public sealed record ClassWithName(string Name);

    public sealed class ClassWithNameValidator : AbstractValidator<ClassWithName>
    {
        public ClassWithNameValidator()
        {
            RuleFor(x => x.Name).IsValidNamespaceName();
        }
    }

    #endregion [ Name ]

    #region [ DisplayName ]

    /// <summary>
    /// Test the following Functional Requirements for Namespace display names:
    /// - **FR-017**: System MUST trim and validate display names with maximum 80 characters
    /// </summary>
    [Theory]
    [MemberData(nameof(GetDisplayNameValidationTestData))]
    public async Task DisplayNameValidationTest(DisplayNameValidationTestData test)
    {
        var validator = new ClassWithDisplayNameValidator();

        var input = new ClassWithDisplayName(test.DisplayName);

        var result = await validator.ValidateAsync(input);

        var nameErrors = result
            .Errors.Where(e => e.PropertyName == nameof(ClassWithDisplayName.DisplayName))
            .ToArray();

        if (test.ExpectedOk)
        {
            nameErrors.ShouldBeEmpty();
        }
        else
        {
            nameErrors.ShouldSatisfyAllConditions(
                () => nameErrors.ShouldNotBeEmpty(),
                () => nameErrors.ShouldContain(e => e.ErrorMessage.Contains(test.ErrorSnippet!))
            );
        }
    }

    public sealed record DisplayNameValidationTestData(
        string? DisplayName,
        bool ExpectedOk = true,
        [property: MemberNotNullWhen(false, nameof(DisplayNameValidationTestData.ExpectedOk))]
            string? ErrorSnippet = null
    );

    public static TheoryData<DisplayNameValidationTestData> GetDisplayNameValidationTestData()
    {
        return
        [
            // Valid display names
            new(""),
            new(null),
            new("My namespace"),
            new("User-Service"),
            new(GenerateDigits(80)),
            // Invalid display names
            new(GenerateDigits(81), false, "length"),
            new(" \r\n\tMy namespace", false, "whitespace"),
            new("My namespace \r\n\t", false, "whitespace"),
            new("   ", false, "whitespace"),
        ];
    }

    public sealed record ClassWithDisplayName(string? DisplayName);

    public sealed class ClassWithDisplayNameValidator : AbstractValidator<ClassWithDisplayName>
    {
        public ClassWithDisplayNameValidator()
        {
            RuleFor(x => x.DisplayName).IsValidNamespaceDisplayName();
        }
    }

    #endregion [ DisplayName ]

    #region [ Description ]

    /// <summary>
    /// Test the following Functional Requirements for Namespace display names:
    /// - **FR-018**: System MUST trim and validate descriptions with maximum 1000 characters
    /// </summary>
    [Theory]
    [MemberData(nameof(GetDescriptionValidationTestData))]
    public async Task DescriptionValidationTest(DescriptionValidationTestData test)
    {
        var validator = new ClassWithDescriptionValidator();

        var input = new ClassWithDescription(test.Description);

        var result = await validator.ValidateAsync(input);

        var nameErrors = result
            .Errors.Where(e => e.PropertyName == nameof(ClassWithDescription.Description))
            .ToArray();

        if (test.ExpectedOk)
        {
            nameErrors.ShouldBeEmpty();
        }
        else
        {
            nameErrors.ShouldSatisfyAllConditions(
                () => nameErrors.ShouldNotBeEmpty(),
                () => nameErrors.ShouldContain(e => e.ErrorMessage.Contains(test.ErrorSnippet!))
            );
        }
    }

    public sealed record DescriptionValidationTestData(
        string? Description,
        bool ExpectedOk = true,
        [property: MemberNotNullWhen(false, nameof(DescriptionValidationTestData.ExpectedOk))]
            string? ErrorSnippet = null
    );

    public static TheoryData<DescriptionValidationTestData> GetDescriptionValidationTestData()
    {
        return
        [
            // Valid display names
            new(""),
            new(null),
            new("My usual description"),
            new(
                "Description can have any kind of content, with spaces, UPPERCASE letters, numbers 1234567890 and special characters !@#$%^&*()"
            ),
            new(GenerateDigits(1000)),
            // Invalid display names
            new(GenerateDigits(1001), false, "length"),
            new(" \r\n\tDescription", false, "whitespace"),
            new("Description \r\n\t", false, "whitespace"),
            new("   ", false, "whitespace"),
        ];
    }

    public sealed record ClassWithDescription(string? Description);

    public sealed class ClassWithDescriptionValidator : AbstractValidator<ClassWithDescription>
    {
        public ClassWithDescriptionValidator()
        {
            RuleFor(x => x.Description).IsValidNamespaceDescription();
        }
    }

    #endregion [ Description ]

    #region [ Documentation ]

    /// <summary>
    /// Test the following Functional Requirements for Namespace display names:
    /// - **FR-018**: System MUST trim and validate descriptions with maximum 1000 characters
    /// </summary>
    [Theory]
    [MemberData(nameof(GetDocumentationValidationTestData))]
    public async Task DocumentationValidationTest(DocumentationValidationTestData test)
    {
        var validator = new ClassWithDocumentationValidator();

        var input = new ClassWithDocumentation(test.Documentation);

        var result = await validator.ValidateAsync(input);

        var nameErrors = result
            .Errors.Where(e => e.PropertyName == nameof(ClassWithDocumentation.Documentation))
            .ToArray();

        if (test.ExpectedOk)
        {
            nameErrors.ShouldBeEmpty();
        }
        else
        {
            nameErrors.ShouldSatisfyAllConditions(
                () => nameErrors.ShouldNotBeEmpty(),
                () => nameErrors.ShouldContain(e => e.ErrorMessage.Contains(test.ErrorSnippet!))
            );
        }
    }

    public sealed record DocumentationValidationTestData(
        string? Documentation,
        bool ExpectedOk = true,
        [property: MemberNotNullWhen(false, nameof(DocumentationValidationTestData.ExpectedOk))]
            string? ErrorSnippet = null
    );

    public static TheoryData<DocumentationValidationTestData> GetDocumentationValidationTestData()
    {
        return
        [
            // Valid display names
            new(""),
            new(null),
            new("Short documentation"),
            new(
                """
                # Namespace Documentation

                You can use **Markdown** here to format your documentation.

                Or even include code snippets:

                ```csharp
                Console.WriteLine("Hello, World!");
                ```

                HTML content is also supported:

                <div style="color: blue;">This text is blue.</div>
                """
            ),
            // Invalid display names
            // Documentation has a very long limit of 10,000 characters. Theory data would be too large.
        ];
    }

    public sealed record ClassWithDocumentation(string? Documentation);

    public sealed class ClassWithDocumentationValidator : AbstractValidator<ClassWithDocumentation>
    {
        public ClassWithDocumentationValidator()
        {
            RuleFor(x => x.Documentation).IsValidNamespaceDocumentation();
        }
    }

    #endregion [ Documentation ]

    private const string TenDigits = "1234567890";

    private static string GenerateDigits(int amount)
    {
        if (amount <= 10)
        {
            return TenDigits[..amount];
        }

        var sb = new StringBuilder(amount);
        var whole = amount / 10;
        var rest = amount % 10;
        for (int i = 0; i < whole; i++)
        {
            sb.Append(TenDigits);
        }
        sb.Append(TenDigits[..rest]);
        return sb.ToString();
    }
}

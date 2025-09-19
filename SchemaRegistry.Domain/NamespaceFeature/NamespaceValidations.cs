using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentValidation;

namespace SchemaRegistry.Domain.NamespaceFeature;

public static partial class NamespaceValidations
{
    // Name

    public const int MaxNameLength = 40;

    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string NamePattern = @"^([a-z][a-z0-9]*)(\-([a-z][a-z0-9]*))*$";

    [GeneratedRegex(NamePattern)]
    private static partial Regex NameRegex();

    public static IRuleBuilder<T, string> IsValidNamespaceName<T>(
        this IRuleBuilderInitial<T, string> ruleBuilder
    ) =>
        ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(MaxNameLength)
            .Matches(NameRegex())
            .WithMessage((_, _) => Resources.Namespace_Validation_Name_PatternMessage);

    // DisplayName

    public const int MaxDisplayNameLength = 100;

    public static IRuleBuilder<T, string?> IsValidNamespaceDisplayName<T>(
        this IRuleBuilderInitial<T, string?> ruleBuilder
    ) => ruleBuilder.Cascade(CascadeMode.Stop).MaximumLength(MaxDisplayNameLength);

    // Description

    public const int MaxDescriptionLength = 1_000;

    public static IRuleBuilder<T, string?> IsValidNamespaceDescription<T>(
        this IRuleBuilderInitial<T, string?> ruleBuilder
    ) => ruleBuilder.Cascade(CascadeMode.Stop).MaximumLength(MaxDescriptionLength);

    // Documentation

    public const int MaxDocumentationLength = 10_000;

    public static IRuleBuilder<T, string?> IsValidNamespaceDocumentation<T>(
        this IRuleBuilderInitial<T, string?> ruleBuilder
    ) => ruleBuilder.Cascade(CascadeMode.Stop).MaximumLength(MaxDocumentationLength);
}

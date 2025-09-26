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

    public static IRuleBuilder<T, string> IsValidOptionalNamespaceName<T>(
        this IRuleBuilder<T, string> ruleBuilder
    ) =>
        ruleBuilder
            .MaximumLength(MaxNameLength)
            .Matches(NameRegex())
            .WithMessage((_, _) => Resources.Namespace_Validation_Name_PatternMessage);

    public static IRuleBuilder<T, string> IsValidNamespaceName<T>(
        this IRuleBuilder<T, string> ruleBuilder
    ) => ruleBuilder.NotEmpty().IsValidOptionalNamespaceName();

    public static IRuleBuilder<T, string> IsValidNamespaceName<T>(
        this IRuleBuilderInitial<T, string> ruleBuilder
    ) => ((IRuleBuilder<T, string>)ruleBuilder.Cascade(CascadeMode.Stop)).IsValidNamespaceName();

    // DisplayName

    public const int MaxDisplayNameLength = 80;

    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string NoTrailingWhitespacesPattern = @"^([^\s](.*[^\s])?)?$";

    [GeneratedRegex(NoTrailingWhitespacesPattern)]
    private static partial Regex NoTrailingWhitespacesRegex();

    public static IRuleBuilder<T, string?> IsValidNamespaceDisplayName<T>(
        this IRuleBuilderInitial<T, string?> ruleBuilder
    ) =>
        ruleBuilder
            .Cascade(CascadeMode.Stop)
            .MaximumLength(MaxDisplayNameLength)
            .Matches(NoTrailingWhitespacesRegex())
            .WithMessage((_, _) => Resources.NoTrailingWhitespaces_Validation_PatternMessage);

    // Description

    public const int MaxDescriptionLength = 1_000;

    public static IRuleBuilder<T, string?> IsValidNamespaceDescription<T>(
        this IRuleBuilderInitial<T, string?> ruleBuilder
    ) =>
        ruleBuilder
            .Cascade(CascadeMode.Stop)
            .MaximumLength(MaxDescriptionLength)
            .Matches(NoTrailingWhitespacesRegex())
            .WithMessage((_, _) => Resources.NoTrailingWhitespaces_Validation_PatternMessage);

    // Documentation

    public const int MaxDocumentationLength = 10_000;

    public static IRuleBuilder<T, string?> IsValidNamespaceDocumentation<T>(
        this IRuleBuilderInitial<T, string?> ruleBuilder
    ) => ruleBuilder.Cascade(CascadeMode.Stop).MaximumLength(MaxDocumentationLength);
}

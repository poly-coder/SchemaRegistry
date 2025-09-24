using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record UpdateNamespaceDocumentationCommand(string Name, string? Documentation = null);

// Validation

internal class UpdateNamespaceDocumentationCommandValidator
    : AbstractValidator<UpdateNamespaceDocumentationCommand>
{
    public UpdateNamespaceDocumentationCommandValidator()
    {
        RuleFor(x => x.Name).IsValidNamespaceName();

        RuleFor(x => x.Documentation).IsValidNamespaceDocumentation();
    }
}

// Extensions

public static class UpdateNamespaceDocumentationCommandExtensions
{
    public static UpdateNamespaceDocumentationCommand Coerce(
        this UpdateNamespaceDocumentationCommand command
    )
    {
        return new(command.Name.CoerceTrim(), command.Documentation.CoerceTrim());
    }
}

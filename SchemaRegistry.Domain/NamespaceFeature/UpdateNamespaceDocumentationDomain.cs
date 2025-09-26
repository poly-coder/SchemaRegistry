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
        var name = command.Name.CoerceTrimRequired();

        if (name == command.Name)
        {
            return command;
        }

        return command with
        {
            Name = name,
        };
    }
}

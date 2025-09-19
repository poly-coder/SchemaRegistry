using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record CreateNamespaceCommand(
    string Name,
    string? DisplayName = null,
    string? Description = null,
    string? Documentation = null
);

public sealed record CreateNamespaceCommandResult;

// Validation

internal class CreateNamespaceCommandValidator : AbstractValidator<CreateNamespaceCommand>
{
    public CreateNamespaceCommandValidator()
    {
        RuleFor(x => x.Name).IsValidNamespaceName();

        RuleFor(x => x.DisplayName).IsValidNamespaceDisplayName();

        RuleFor(x => x.Description).IsValidNamespaceDescription();

        RuleFor(x => x.Documentation).IsValidNamespaceDocumentation();
    }
}

// Extensions

public static class CreateNamespaceCommandExtensions
{
    public static CreateNamespaceCommand Coerce(this CreateNamespaceCommand command)
    {
        return new(
            command.Name.CoerceTrim(),
            command.DisplayName.CoerceTrim(),
            command.Description.CoerceTrim(),
            command.Documentation.CoerceTrim()
        );
    }
}

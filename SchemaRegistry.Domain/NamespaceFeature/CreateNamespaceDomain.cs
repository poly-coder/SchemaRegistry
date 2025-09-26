using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record CreateNamespaceCommand(
    string Name,
    string? DisplayName = null,
    string? Description = null,
    string? Documentation = null
);

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
        var name = command.Name.CoerceTrimRequired();
        var displayName = command.DisplayName.CoerceTrim();
        var description = command.Description.CoerceTrim();

        if (
            name == command.Name
            && displayName == command.DisplayName
            && description == command.Description
        )
        {
            return command;
        }

        return command with
        {
            Name = name,
            DisplayName = displayName,
            Description = description,
        };
    }
}

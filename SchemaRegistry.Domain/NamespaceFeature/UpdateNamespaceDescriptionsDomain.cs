using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record UpdateNamespaceDescriptionsCommand(
    string Name,
    string? DisplayName = null,
    string? Description = null
);

// Validation

internal class UpdateNamespaceDescriptionsCommandValidator
    : AbstractValidator<UpdateNamespaceDescriptionsCommand>
{
    public UpdateNamespaceDescriptionsCommandValidator()
    {
        RuleFor(x => x.Name).IsValidNamespaceName();

        RuleFor(x => x.DisplayName).IsValidNamespaceDisplayName();

        RuleFor(x => x.Description).IsValidNamespaceDescription();
    }
}

// Extensions

public static class UpdateNamespaceDescriptionsCommandExtensions
{
    public static UpdateNamespaceDescriptionsCommand Coerce(
        this UpdateNamespaceDescriptionsCommand command
    )
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

        return new(Name: name, DisplayName: displayName, Description: description);
    }
}

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
        return new(
            command.Name.CoerceTrim(),
            command.DisplayName.CoerceTrim(),
            command.Description.CoerceTrim()
        );
    }
}

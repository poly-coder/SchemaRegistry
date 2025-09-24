using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record RestoreNamespaceCommand(string Name);

// Validation

internal class RestoreNamespaceCommandValidator : AbstractValidator<RestoreNamespaceCommand>
{
    public RestoreNamespaceCommandValidator()
    {
        RuleFor(x => x.Name).IsValidNamespaceName();
    }
}

// Extensions

public static class RestoreNamespaceCommandExtensions
{
    public static RestoreNamespaceCommand Coerce(this RestoreNamespaceCommand command)
    {
        return new(command.Name.CoerceTrim());
    }
}

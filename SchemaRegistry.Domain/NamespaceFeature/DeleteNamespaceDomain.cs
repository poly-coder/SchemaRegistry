using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record DeleteNamespaceCommand(string Name, bool Permanently);

public sealed record DeleteNamespaceCommandResult(bool PermanentlyDeleted);

// Validation

internal class DeleteNamespaceCommandValidator : AbstractValidator<DeleteNamespaceCommand>
{
    public DeleteNamespaceCommandValidator()
    {
        RuleFor(x => x.Name).IsValidNamespaceName();
    }
}

// Extensions

public static class DeleteNamespaceCommandExtensions
{
    public static DeleteNamespaceCommand Coerce(this DeleteNamespaceCommand command)
    {
        return command with { Name = command.Name.CoerceTrim() };
    }
}

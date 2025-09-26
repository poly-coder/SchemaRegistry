using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record DeleteNamespaceCommand(string Name);

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
        var name = command.Name.CoerceTrimRequired();

        if (name == command.Name)
        {
            return command;
        }

        return new(Name: name);
    }
}

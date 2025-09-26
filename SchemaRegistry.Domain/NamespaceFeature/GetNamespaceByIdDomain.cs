using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record GetNamespaceByIdQuery(string Name, bool Deleted = false);

public sealed record GetNamespaceByIdQueryResult(NamespaceDetailsInfo Namespace);

// Validation

internal class GetNamespaceByIdQueryValidator : AbstractValidator<GetNamespaceByIdQuery>
{
    public GetNamespaceByIdQueryValidator()
    {
        RuleFor(x => x.Name).IsValidNamespaceName();
    }
}

// Extensions

public static class GetNamespaceByIdQueryExtensions
{
    public static GetNamespaceByIdQuery Coerce(this GetNamespaceByIdQuery command)
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

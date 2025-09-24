using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record GetNamespaceByIdQuery(string Name, bool Deleted);

public sealed record GetNamespaceByIdQueryResult(
    NamespaceDetails Details,
    NamespaceOperations Operations
);

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
        return command with { Name = command.Name.CoerceTrim() };
    }
}

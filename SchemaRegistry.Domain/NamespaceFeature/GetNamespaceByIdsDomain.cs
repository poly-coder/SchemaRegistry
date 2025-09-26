using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record GetNamespaceByIdsQuery(string Name, bool Deleted);

public sealed record GetNamespaceByIdsQueryResult(IReadOnlyList<NamespaceDetailsInfo> Namespaces);

// Validation

internal class GetNamespaceByIdsQueryValidator : AbstractValidator<GetNamespaceByIdsQuery>
{
    public GetNamespaceByIdsQueryValidator()
    {
        RuleFor(x => x.Name).IsValidNamespaceName();
    }
}

// Extensions

public static class GetNamespaceByIdsQueryExtensions
{
    public static GetNamespaceByIdsQuery Coerce(this GetNamespaceByIdsQuery command)
    {
        return command with { Name = command.Name.CoerceTrim() };
    }
}

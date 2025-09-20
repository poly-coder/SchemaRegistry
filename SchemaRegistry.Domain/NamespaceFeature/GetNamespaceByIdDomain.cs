using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record GetNamespaceByIdQuery(string Name, bool Deleted);

public sealed record GetNamespaceByIdQueryResult(
    NamespaceDetails Details,
    NamespaceOperations Operations
);

public sealed record NamespaceDetails(
    string Name,
    string? DisplayName,
    string? Description,
    string? Documentation,
    DateTimeOffset CreatedAt,
    DateTimeOffset? DeletedAt,
    bool IsDeleted
);

public sealed record NamespaceOperations(
    bool CanUpdateDescriptions,
    bool CanDelete,
    bool CanRestore,
    bool CanDeletePermanently
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

using System.Text;
using FluentValidation;
using Pico;

namespace SchemaRegistry.Domain.NamespaceFeature;

public sealed record GetNamespaceByIdsQuery(IReadOnlyList<string> Names, bool Deleted = false)
{
    public bool Equals(GetNamespaceByIdsQuery? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Deleted == other.Deleted && Names.SequenceEqual(other.Names);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Deleted, Names.Count);
    }

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append(nameof(Names)).Append(" = [").Append(string.Join(", ", Names)).Append("], ");
        builder.Append(nameof(Deleted)).Append(" = ").Append(Deleted);
        return true;
    }
}

public sealed record GetNamespaceByIdsQueryResult(IReadOnlyList<NamespaceDetailsInfo> Namespaces);

// Validation

internal class GetNamespaceByIdsQueryValidator : AbstractValidator<GetNamespaceByIdsQuery>
{
    public GetNamespaceByIdsQueryValidator()
    {
        RuleFor(x => x.Names).NotNull();
        RuleForEach(x => x.Names).IsValidNamespaceName();
    }
}

// Extensions

public static class GetNamespaceByIdsQueryExtensions
{
    public static GetNamespaceByIdsQuery Coerce(this GetNamespaceByIdsQuery command)
    {
        var names = command.Names.CoerceTrimRequired();

        if (names == command.Names)
        {
            return command;
        }

        return command with
        {
            Names = names,
        };
    }
}

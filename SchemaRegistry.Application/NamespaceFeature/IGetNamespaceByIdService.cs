using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IGetNamespaceByIdService
{
    Task<GetNamespaceByIdQueryResult> GetNamespaceByIdAsync(
        GetNamespaceByIdQuery command,
        CancellationToken cancel = default
    );
}

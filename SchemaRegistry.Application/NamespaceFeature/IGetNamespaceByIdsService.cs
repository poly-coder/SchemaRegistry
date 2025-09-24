using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IGetNamespaceByIdsService
{
    Task<GetNamespaceByIdsQueryResult> GetNamespaceByIdsAsync(
        GetNamespaceByIdsQuery command,
        CancellationToken cancel = default
    );
}
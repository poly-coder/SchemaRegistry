using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IGetNamespaceByIdsUseCase
{
    Task<GetNamespaceByIdsQueryResult> GetNamespaceByIdsAsync(
        GetNamespaceByIdsQuery command,
        CancellationToken cancel = default
    );
}

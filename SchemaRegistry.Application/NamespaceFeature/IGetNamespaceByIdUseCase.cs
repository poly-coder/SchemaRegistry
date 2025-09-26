using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IGetNamespaceByIdUseCase
{
    Task<GetNamespaceByIdQueryResult> GetNamespaceByIdAsync(
        GetNamespaceByIdQuery command,
        CancellationToken cancel = default
    );
}

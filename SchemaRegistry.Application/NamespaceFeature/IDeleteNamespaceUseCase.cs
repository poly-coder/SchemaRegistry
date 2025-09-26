using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IDeleteNamespaceUseCase
{
    Task<NamespaceCommandResult> DeleteNamespaceAsync(
        DeleteNamespaceCommand command,
        CancellationToken cancel = default
    );
}

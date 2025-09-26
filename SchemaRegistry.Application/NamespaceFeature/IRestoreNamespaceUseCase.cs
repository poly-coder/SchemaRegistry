using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IRestoreNamespaceUseCase
{
    Task<NamespaceCommandResult> RestoreNamespaceAsync(
        RestoreNamespaceCommand command,
        CancellationToken cancel = default
    );
}

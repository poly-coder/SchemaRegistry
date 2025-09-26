using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface ICreateNamespaceUseCase
{
    Task<NamespaceCommandResult> CreateNamespaceAsync(
        CreateNamespaceCommand command,
        CancellationToken cancel = default
    );
}

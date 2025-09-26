using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IUpdateNamespaceDescriptionsUseCase
{
    Task<NamespaceCommandResult> UpdateNamespaceDescriptionsAsync(
        UpdateNamespaceDescriptionsCommand command,
        CancellationToken cancel = default
    );
}

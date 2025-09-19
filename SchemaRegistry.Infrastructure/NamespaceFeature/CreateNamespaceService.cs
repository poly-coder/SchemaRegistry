using SchemaRegistry.Application.NamespaceFeature;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

internal sealed class CreateNamespaceService(IGrainFactory grains) : ICreateNamespaceService
{
    public async Task<CreateNamespaceCommandResult> CreateNamespaceAsync(
        CreateNamespaceCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToInput();

        var output = await grain.CreateNamespace(input);

        return output.MapToResult();
    }
}

// Mapper

internal static class CreateNamespaceServiceMapper
{
    internal static CreateNamespaceInput MapToInput(this CreateNamespaceCommand command) =>
        new(command.Name, command.DisplayName, command.Description, command.Documentation);

    internal static CreateNamespaceCommandResult MapToResult(this CreateNamespaceOutput output) =>
        new();
}

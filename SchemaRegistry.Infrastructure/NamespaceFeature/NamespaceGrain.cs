using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public sealed class NamespaceGrain : Grain, INamespaceGrain
{
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);
    }

    public async Task<CreateNamespaceOutput> CreateNamespace(CreateNamespaceInput command)
    {
        await Task.CompletedTask;

        return new();
    }
}

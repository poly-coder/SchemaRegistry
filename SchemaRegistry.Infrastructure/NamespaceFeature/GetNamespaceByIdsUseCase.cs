using Pico.Domain.Errors;
using SchemaRegistry.Application.NamespaceFeature;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

internal sealed class GetNamespaceByIdsUseCase(IGetNamespaceByIdUseCase getById)
    : IGetNamespaceByIdsUseCase
{
    public async Task<GetNamespaceByIdsQueryResult> GetNamespaceByIdsAsync(
        GetNamespaceByIdsQuery command,
        CancellationToken cancel = default
    )
    {
        var tasks = command.Names.Select(async name =>
        {
            try
            {
                var singleResult = await getById.GetNamespaceByIdAsync(
                    new Domain.NamespaceFeature.GetNamespaceByIdQuery(
                        Name: name,
                        Deleted: command.Deleted
                    ),
                    cancel
                );

                return singleResult.Namespace;
            }
            catch (EntityNotFoundException)
            {
                return null;
            }
        });

        var namespaces = (await Task.WhenAll(tasks))
            .OfType<Domain.NamespaceFeature.NamespaceDetailsInfo>()
            .ToArray();

        return new GetNamespaceByIdsQueryResult(namespaces);
    }
}

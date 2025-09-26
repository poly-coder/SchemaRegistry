using SchemaRegistry.WebApi.NamespaceFeature.RestApis;

namespace SchemaRegistry.WebApi.NamespaceFeature;

internal static class RestEndpoints
{
    public const string BaseApiPath = "/api";

    public static IEndpointRouteBuilder MapSchemaRegistryEndpoints(
        this IEndpointRouteBuilder endpoints
    )
    {
        var api = endpoints.MapGroup(BaseApiPath);

        api.MapNamespaceEndpoints();

        return endpoints;
    }
}

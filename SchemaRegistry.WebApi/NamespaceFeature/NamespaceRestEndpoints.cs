namespace SchemaRegistry.WebApi.NamespaceFeature;

internal static class NamespaceRestEndpoints
{
    public const string NamespaceBasePath = "/ns";

    public static IEndpointRouteBuilder MapNamespaceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(NamespaceBasePath).WithTags("Namespace");

        group.MapCreateNamespaceEndpoints();

        return endpoints;
    }
}

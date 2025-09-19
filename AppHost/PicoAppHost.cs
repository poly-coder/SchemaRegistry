namespace Pico.AppHost.Postgres;

[Flags]
public enum PicoPostgresUI
{
    None = 0,

    PgAdmin = 1 << 0,
    PgWeb = 1 << 1,

    All = PgAdmin | PgWeb,
}

public static class PicoAppHostExtensions
{
    public static IResourceBuilder<PostgresServerResource> AddPicoPostgres(
        this IDistributedApplicationBuilder builder,
        [ResourceName] string name,
        IResourceBuilder<ParameterResource>? userName = null,
        IResourceBuilder<ParameterResource>? password = null,
        int? port = null,
        bool isTesting = false,
        PicoPostgresUI ui = PicoPostgresUI.None,
        ContainerLifetime lifetime = ContainerLifetime.Session
    )
    {
        var resource = builder.AddPostgres(name, userName, password, port);

        if (!isTesting)
        {
            resource.WithDataVolume().WithLifetime(lifetime);
        }
        else
        {
            resource.WithLifetime(ContainerLifetime.Session);
        }

        if (ui != PicoPostgresUI.None)
        {
            if ((ui & PicoPostgresUI.PgAdmin) != 0)
            {
                resource.WithPgAdmin();
            }

            if ((ui & PicoPostgresUI.PgWeb) != 0)
            {
                resource.WithPgWeb();
            }
        }

        return resource;
    }
}

using Pico.AppHost.Postgres;

var builder = DistributedApplication.CreateBuilder(args);

var isTesting = builder.Configuration["IsTesting"] == "true";

var postgres = builder.AddPicoPostgres("postgres", isTesting: isTesting, ui: PicoPostgresUI.All);

var database = postgres.AddDatabase("database", "postgres");

builder
    .AddProject<Projects.SchemaRegistry_WebApi>("schema-registry")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();

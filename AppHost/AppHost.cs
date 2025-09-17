var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SchemaRegistry_WebApi>("schema-registry");

builder.Build().Run();

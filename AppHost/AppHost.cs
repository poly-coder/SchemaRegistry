var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SchemaRegistry_WebApi>("schemaregistry-webapi");

builder.Build().Run();

using SchemaRegistry.WebApi;

var builder = WebApplication.CreateBuilder(args);

Startup.ConfigureServices(builder);

var app = builder.Build();

Startup.ConfigureApplication(app);

app.Run();

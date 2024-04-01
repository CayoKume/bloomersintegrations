using BloomersIntegrationsManager.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddArchitectures()
    .AddServices();

var app = builder.Build();

app.UseApplication();

app.MapControllers();

app.Run();
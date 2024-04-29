using BloomersIntegrationsManager.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddArchitectures()
    .AddServices();

var app = builder.Build();

app.UseApplication(builder.Configuration.GetSection("ConfigureServer").GetSection("ServerName").Value);
app.MapControllers();

app.Run();
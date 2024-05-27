using BloomersIntegrationsManager.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);
var serverName = builder.Configuration.GetSection("ConfigureServer").GetSection("ServerName").Value;

builder
    .AddArchitectures(serverName)
    .AddServices();

var app = builder.Build();

app.UseApplication(serverName);

if (serverName == "SRV-VM-APP03")
    app.MapControllers();

app.Run();
using BloomersIntegrationsManager.Domain.Extensions;
using BloomersIntegrationsManager.Domain.RecurringJobs.Carriers;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddArchitectures()
    .AddServices();

var app = builder.Build();

app.UseApplication();

//FlashCourierJobs.AddFlashCourierJobs();

app.MapControllers();

app.Run();
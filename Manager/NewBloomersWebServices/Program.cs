using BloomersCarriersIntegrations.FlashCourier.Application.Services;
using BloomersIntegrationsManager.Domain.Extensions;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddArchitectures()
    .AddServices();

//RecurringJobsExtensions.AddRecurringJobs();

var app = builder.Build();

app.UseApplication();

app.MapControllers();

app.Run();
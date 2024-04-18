using BloomersWorkersManager.Domain.Extensions;
using Serilog;
using Serilog.Events;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.AddServices();

        Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .WriteTo.Console()
                    .WriteTo.File($@"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/logs/InvoiceBot-.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

        var host = builder
            .UseWindowsService()
            .Build();

        host.Run();
    }
}
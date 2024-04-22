using BloomersWorkers.AuthorizeNFe.Application.Services;
using BloomersWorkers.ChangingOrder.Application.Services;
using BloomersWorkers.ChangingPassword.Application.Services;
using BloomersWorkers.InsertReverse.Application.Services;
using BloomersWorkers.InvoiceOrder.Application.Services;
using BloomersWorkers.LabelsPrinter.Application.Services;
using Serilog;
using System.Reflection;

namespace BloomersWorkersManager;

public class Worker : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _lifetime;

    public Worker
        (IServiceProvider serviceProvider, IHostApplicationLifetime lifetime, IConfiguration configuration) =>
        (_serviceProvider, _lifetime, _configuration) = (serviceProvider, lifetime, configuration);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            IAuthorizeNFeService _authorizeNFeService = scope.ServiceProvider.GetRequiredService<IAuthorizeNFeService>();
            IChangingOrderService _changingOrderService = scope.ServiceProvider.GetRequiredService<IChangingOrderService>();
            IChangingPasswordService _changingPasswordService = scope.ServiceProvider.GetRequiredService<IChangingPasswordService>();
            //IInsertReverseService _insertReverseService = scope.ServiceProvider.GetRequiredService<IInsertReverseService>();
            IInvoiceOrderService _invoiceOrderService = scope.ServiceProvider.GetRequiredService<IInvoiceOrderService>();
            ILabelsPrinterService _labelsPrinterService = scope.ServiceProvider.GetRequiredService<ILabelsPrinterService>();

            try
            {
                string? workerName = _configuration.GetSection("ConfigureService").GetSection("WorkerName").Value;
                while (!stoppingToken.IsCancellationRequested)
                {
                    switch (workerName)
                    {
                        case "AuthorizeNFe":
                            await _authorizeNFeService.AuthorizeNFes();
                            break;
                        case "ChangingOrder":
                            await _changingOrderService.ChangingOrder();
                            break;
                        case "ChangingPassword":
                            await _changingPasswordService.ChangePassword();
                            break;
                        //case "InsertReverse":
                        //    await _insertReverseService.InsereReversa();
                        //    break;
                        case "InvoiceOrder":
                            await _invoiceOrderService.InvoiceOrder(Assembly.GetExecutingAssembly().GetName().Name);
                            break;
                        case "LabelsPrinter":
                            await _labelsPrinterService.PrintLabels();
                            break;
                        default:
                            break;
                    }
                    await Task.Delay(2 * 1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Log.Information($"StopAplication: {ex.Message}");
                _lifetime.StopApplication();
            }
        }
    }
}

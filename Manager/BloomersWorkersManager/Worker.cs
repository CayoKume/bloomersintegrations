using BloomersWorkers.AuthorizeNFe.Application.Services;
using BloomersWorkers.ChangingOrder.Application.Services;
using BloomersWorkers.ChangingPassword.Application.Services;
using BloomersWorkers.InsertReverse.Application.Services;
using BloomersWorkers.InvoiceOrder.Application.Services;
using BloomersWorkers.LabelsPrinter.Application.Services;

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
                            return;
                        case "ChangingOrder":
                            await _changingOrderService.ChangingOrder();
                            return;
                        case "ChangingPassword":
                            await _changingPasswordService.ChangePassword();
                            return;
                        //case "InsertReverse":
                        //    await _insertReverseService.InsereReversa();
                        //    return;
                        case "InvoiceOrder":
                            await _invoiceOrderService.InvoiceOrder();
                            return;
                        case "LabelsPrinter":
                            await _labelsPrinterService.PrintLabels();
                            return;
                        default:
                            break;
                    }
                    await Task.Delay(30 * 1000, stoppingToken);
                }
            }
            catch
            {
                _lifetime.StopApplication();
            }
        }
    }
}

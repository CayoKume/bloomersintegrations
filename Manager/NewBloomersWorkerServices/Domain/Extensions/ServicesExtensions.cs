using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersWorkers.AuthorizeNFe.Application.Services;
using BloomersWorkers.AuthorizeNFe.Infrastructure.Repositorys;
using BloomersWorkers.AuthorizeNFe.Infrastructure.Source.Pages;
using BloomersWorkers.ChangingOrder.Application.Services;
using BloomersWorkers.ChangingOrder.Infrastructure.Repositorys;
using BloomersWorkers.ChangingOrder.Infrastructure.Source.Pages;
using BloomersWorkers.ChangingPassword.Application.Services;
using BloomersWorkers.ChangingPassword.Infrastructure.Repositorys;
using BloomersWorkers.ChangingPassword.Infrastructure.Source.Pages;
using BloomersWorkers.InvoiceOrder.Application.Services;
using BloomersWorkers.InvoiceOrder.Infrastructure.Repositorys;
using BloomersWorkers.InvoiceOrder.Infrastructure.Source.Pages;
using BloomersWorkers.LabelsPrinter.Application.Services;
using BloomersWorkers.LabelsPrinter.Infrastructure.Apis;
using BloomersWorkers.LabelsPrinter.Infrastructure.Repositorys;
using BloomersWorkersCore.Infrastructure.Repositorys;
using BloomersWorkersCore.Infrastructure.Source.Drivers;
using BloomersWorkersCore.Infrastructure.Source.Pages;

namespace BloomersWorkersManager.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddHostedServices();
                services.AddScopedSQLServerConnection();
                services.AddScopedWorkersCoreServices();
                services.AddScopedAuthorizeNFeServices();
                services.AddScopedChangingOrderServices();
                services.AddScopedChangingPasswordServices();
                services.AddScopedInsertReverseServices();
                services.AddScopedInvoiceOrderServices();
                services.AddScopedLabelsPrinterServices();
            });

            return builder;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<Worker>();
            return services;
        }

        public static IServiceCollection AddScopedWorkersCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IBloomersWorkersCoreRepository, BloomersWorkersCoreRepository>();
            services.AddScoped<IChromeDriver, ChromeDriver>();
            services.AddScoped<IHomePage, HomePage>();
            services.AddScoped<ILoginPage, LoginPage>();
            return services;
        }

        public static IServiceCollection AddScopedSQLServerConnection(this IServiceCollection services)
        {
            services.AddScoped<ISQLServerConnection, SQLServerConnection>();

            return services;
        }

        public static IServiceCollection AddScopedAuthorizeNFeServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizeNFePage, AuthorizeNFePage>();
            services.AddScoped<IAuthorizeNFeService, AuthorizeNFeService>();
            services.AddScoped<IAuthorizeNFeRepository, AuthorizeNFeRepository>();
            return services;
        }

        public static IServiceCollection AddScopedChangingOrderServices(this IServiceCollection services)
        {
            services.AddScoped<IChangingOrderPage, ChangingOrderPage>();
            services.AddScoped<IChangingOrderService, ChangingOrderService>();
            services.AddScoped<IChangingOrderRepository, ChangingOrderRepository>();
            return services;
        }

        public static IServiceCollection AddScopedChangingPasswordServices(this IServiceCollection services)
        {
            services.AddScoped<IChangingPasswordPage, ChangingPasswordPage>();
            services.AddScoped<IChangingPasswordService, ChangingPasswordService>();
            services.AddScoped<IChangingPasswordRepository, ChangingPasswordRepository>();
            return services;
        }

        public static IServiceCollection AddScopedInsertReverseServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddScopedInvoiceOrderServices(this IServiceCollection services)
        {
            services.AddScoped<IVDPage, VDPage>();
            services.AddScoped<IB2CPage, B2CPage>();
            services.AddScoped<IInvoiceOrderService, InvoiceOrderService>();
            services.AddScoped<IInvoiceOrderRepository, InvoiceOrderRepository>();
            return services;
        }

        public static IServiceCollection AddScopedLabelsPrinterServices(this IServiceCollection services)
        {
            services.AddScoped<IAPICall, APICall>();
            services.AddScoped<IGenerateZPLsService, GenerateZPLsService>();
            services.AddScoped<ILabelsPrinterService, LabelsPrinterService>();
            services.AddScoped<ILabelsPrinterRepository, LabelsPrinterRepository>();

            return services;
        }
    }
}

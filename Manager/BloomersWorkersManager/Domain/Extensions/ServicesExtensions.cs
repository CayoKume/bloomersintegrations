using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersWorkers.InvoiceOrder.Application.Services;
using BloomersWorkers.InvoiceOrder.Infrastructure.Repositorys;
using BloomersWorkers.InvoiceOrder.Infrastructure.Source.Pages;
using BloomersWorkers.LabelsPrinter.Application.Services;
using BloomersWorkers.LabelsPrinter.Infrastructure.Apis;
using BloomersWorkers.LabelsPrinter.Infrastructure.Repositorys;
using BloomersWorkersCore.Infrastructure.Source.Drivers;
using BloomersWorkersCore.Infrastructure.Source.Pages;

namespace BloomersWorkersManager.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static HostApplicationBuilder AddServices(this HostApplicationBuilder builder)
        {
            builder.Services.AddHostedServices();
            builder.Services.AddScopedSQLServerConnection();
            builder.Services.AddScopedInvoiceOrderServices();
            builder.Services.AddScopedWorkersCoreServices();
            builder.Services.AddScopedLabelsPrinterServices();

            return builder;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<Worker>();
            return services;
        }

        public static IServiceCollection AddScopedWorkersCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IChromeDriver, ChromeDriver>();
            services.AddScoped<IHomePage, HomePage>();
            services.AddScoped<ILoginPage, LoginPage>();
            return services;
        }

        public static IServiceCollection AddScopedAuthorizeNFeServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddScopedChangingOrderServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddScopedChangingPasswordServices(this IServiceCollection services)
        {
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

        public static IServiceCollection AddScopedSQLServerConnection(this IServiceCollection services)
        {
            services.AddScoped<ISQLServerConnection, SQLServerConnection>();

            return services;
        }
    }
}

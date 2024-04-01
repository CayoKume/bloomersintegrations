using BloomersCarriersIntegrations.FlashCourier.Application.Services;
using BloomersCarriersIntegrations.FlashCourier.Infrastructure.Repositorys;
using BloomersCarriersIntegrations.TotalExpress.Application.Services;
using BloomersCarriersIntegrations.TotalExpress.Infrastructure.Repositorys;
using BloomersCommerceIntegrations.LinxCommerce.Application.Services;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys.Base;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersIntegrationsManager.Domain.Filters;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Http;

namespace BloomersIntegrationsManager.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("Connection");

            builder.Services.AddScopedSQLServerConnection();
            builder.Services.AddScopedCarriersServices();
            builder.Services.AddScopedLinxCommerceServices();
            builder.Services.AddHangfireService(connectionString);

            return builder;
        }

        public static IServiceCollection AddScopedLinxCommerceServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILinxCommerceRepositoryBase<>), typeof(LinxCommerceRepositoryBase<>));
            services.AddScoped<BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Apis.IAPICall, BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Apis.APICall>();
            services.AddHttpClient("LinxCommerceAPI", client =>
            {
                client.BaseAddress = new Uri("https://misha.layer.core.dcg.com.br");
                client.Timeout = new TimeSpan(0, 2, 0);
            });

            services.AddScoped<IOrderService<SearchOrderResponse.Root>, OrderService<SearchOrderResponse.Root>>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IProductService<SearchProductResponse.Root>, ProductService<SearchProductResponse.Root>>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ISKUService<SearchSKUResponse.Root>, SKUService<SearchSKUResponse.Root>>();
            services.AddScoped<ISKURepository, SKURepository>();

            return services;
        }

        public static IServiceCollection AddScopedLinxMicrovixCommerceServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddScopedLinxMicrovixERPServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddScopedWmsServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddScopedCarriersServices(this IServiceCollection services)
        {
            services.AddScoped<BloomersCarriersIntegrations.FlashCourier.Infrastructure.Apis.IAPICall, BloomersCarriersIntegrations.FlashCourier.Infrastructure.Apis.APICall>();
            services.AddHttpClient("FlashCourierAPI", client =>
            {
                //HOMOLOG
                //https://homolog.flashpegasus.com.br/FlashPegasus/rest

                client.BaseAddress = new Uri("https://webservice.flashpegasus.com.br/FlashPegasus/rest");
                client.Timeout = new TimeSpan(0, 2, 0);
            });

            services.AddScoped<IFlashCourierService, FlashCourierService>();
            services.AddScoped<IFlashCourierRepository, FlashCourierRepository>();

            services.AddScoped<BloomersCarriersIntegrations.TotalExpress.Infrastructure.Apis.IAPICall, BloomersCarriersIntegrations.TotalExpress.Infrastructure.Apis.APICall>();
            services.AddHttpClient("TotalExpressAPI", client =>
            {
                client.BaseAddress = new Uri("https://apis.totalexpress.com.br/");
                client.Timeout = new TimeSpan(0, 2, 0);
            });
            services.AddHttpClient("TotalExpressEdiAPI", client =>
            {
                client.BaseAddress = new Uri("https://edi.totalexpress.com.br/");
                client.Timeout = new TimeSpan(0, 2, 0);
            });
            services.AddScoped<ITotalExpressService, TotalExpressService>();
            services.AddScoped<ITotalExpressRepository, TotalExpressRepository>();

            return services;
        }

        public static IServiceCollection AddScopedGeneralServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddScopedSQLServerConnection(this IServiceCollection services)
        {
            services.AddScoped<ISQLServerConnection, SQLServerConnection>();

            return services;
        }

        public static IServiceCollection AddHangfireService(this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(configuration => configuration
                .UseFilter(new AutomaticRetryAttribute { Attempts = 0 })
                .UseFilter(new WorkflowJobFailureAttribute())
                .UseFilter(new DisableConcurrentExecutionWithParametersAttribute())

                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer(options =>
            {
                options.WorkerCount = 50;
            });

            return services;
        }
    }
}

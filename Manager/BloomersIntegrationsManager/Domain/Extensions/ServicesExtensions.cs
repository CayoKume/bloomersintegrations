using BloomersCarriersIntegrations.FlashCourier.Application.Services;
using BloomersCarriersIntegrations.FlashCourier.Infrastructure.Apis;
using BloomersCarriersIntegrations.FlashCourier.Infrastructure.Repositorys;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersIntegrationsManager.Domain.Filters;
using Hangfire;
using Hangfire.SqlServer;

namespace BloomersIntegrationsManager.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("Connection");

            builder.Services.AddScopedSQLServerConnection();
            builder.Services.AddScopedCarriersServices();
            builder.Services.AddHangfireService(connectionString);

            return builder;
        }

        public static IServiceCollection AddScopedLinxCommerceServices(this IServiceCollection services)
        {
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
            services.AddScoped<IAPICall, APICall>();
            services.AddScoped<IFlashCourierService, FlashCourierService>();
            services.AddScoped<IFlashCourierRepository, FlashCourierRepository>();
            
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

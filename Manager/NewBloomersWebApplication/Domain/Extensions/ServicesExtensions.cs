using NewBloomersWebApplication.Application.Services;
using NewBloomersWebApplication.Infrastructure.Apis;

namespace NewBloomersWebApplication.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            services.AddHttpClient("MiniWMS", client =>
            {
                client.BaseAddress = new Uri("http://172.25.1.6:7172/NewBloomers/BloomersInvoiceIntegrations/MiniWms/");
                //client.BaseAddress = new Uri("http://localhost:5118/NewBloomers/BloomersInvoiceIntegrations/MiniWms/");
                client.Timeout = new TimeSpan(0, 2, 0);
            });

            services.AddScoped<IAPICall, APICall>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IDeliveryListService, DeliveryListService>();
            services.AddScoped<ILabelsService, LabelsService>();
            services.AddScoped<IPickingService, PickingService>();

            return services;
        }
    }
}

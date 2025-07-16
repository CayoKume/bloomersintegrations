using NewBloomersWebApplication.Application.Services;
using NewBloomersWebApplication.Infrastructure.Apis;

namespace NewBloomersWebApplication.Infrastructure.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                services.AddHttpClient("MiniWMS", client =>
                {
                    client.BaseAddress = new Uri("http://localhost:5118/NewBloomers/BloomersInvoiceIntegrations/MiniWms/");
                    //client.BaseAddress = new Uri("https://localhost:7049/NewBloomers/BloomersInvoiceIntegrations/MiniWms/");
                    client.Timeout = new TimeSpan(0, 2, 0);
                });
            }
            else
            {
                services.AddHttpClient("MiniWMS", client =>
                {
                    client.BaseAddress = new Uri("https://webservices.newbloomers.com.br:7072/NewBloomers/BloomersInvoiceIntegrations/MiniWms/");
                    client.Timeout = new TimeSpan(0, 2, 0);
                });
            }

            services.AddScoped<IAPICall, APICall>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IDeliveryListService, DeliveryListService>();
            services.AddScoped<ILabelsService, LabelsService>();
            services.AddScoped<IPickingService, PickingService>();
            services.AddScoped<ICancellationRequestService, CancellationRequestService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IExecuteCancellationService, ExecuteCancellationService>();

            return services;
        }
    }
}

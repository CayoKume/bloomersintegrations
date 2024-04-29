using NewBloomersWebApplication.Application.Services;

namespace NewBloomersWebApplication.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            services.AddHttpClient("MiniWMS", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44378/NewBloomers/BloomersInvoiceIntegrations/MiniWms/");
                client.Timeout = new TimeSpan(0, 2, 0);
            });

            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IDeliveryListService, DeliveryListService>();
            services.AddScoped<ILabelsService, LabelsService>();
            services.AddScoped<IPickingService, PickingService>();

            return services;
        }
    }
}

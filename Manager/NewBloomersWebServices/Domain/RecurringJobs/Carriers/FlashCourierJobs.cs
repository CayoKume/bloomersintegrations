using BloomersCarriersIntegrations.FlashCourier.Application.Services;
using Hangfire;

namespace BloomersIntegrationsManager.Domain.RecurringJobs.Carriers
{
    public static class FlashCourierJobs
    {
        public static void AddFlashCourierJobs()
        {
            RecurringJob.AddOrUpdate<IFlashCourierService>("FlashCourierEnviaPedidos", service => service.EnviaPedidosFlash(),
                Cron.MinuteInterval(3)
            );

            RecurringJob.AddOrUpdate<IFlashCourierService>("FlashCourierAtualizaLogPedido", service => service.AtualizaLogPedidoEnviado(),
                Cron.MinuteInterval(5)
            );
        }
    }
}

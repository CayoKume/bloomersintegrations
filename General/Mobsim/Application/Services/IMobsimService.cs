namespace BloomersGeneralIntegrations.Mobsim.Application.Services
{
    public interface IMobsimService
    {
        public Task SendMessageInvoicedOrder();
        public Task SendMessageShippdedOrder();
        public Task SendMessageDeliveredOrder();
    }
}

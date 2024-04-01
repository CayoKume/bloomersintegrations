namespace BloomersCarriersIntegrations.TotalExpress.Application.Services
{
    public interface ITotalExpressService
    {
        public Task<bool> SendOrders();
        public Task<bool> SendOrder(string order_number);
        public Task<bool> SendOrderAsEtur(string order_number);
        public Task UpdateOrderSendLog();
    }
}

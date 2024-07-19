namespace BloomersCarriersIntegrations.Jadlog.Application.Services
{
    public interface IJadlogService
    {
        public Task<bool> SendOrdersJadlog();
        public Task<bool> SendOrderJadlog(string order_number);
        public Task<bool> UpdateShippedOrdersLog();
    }
}

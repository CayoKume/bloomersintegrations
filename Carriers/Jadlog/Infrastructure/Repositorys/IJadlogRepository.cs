using BloomersCarriersIntegrations.Jadlog.Domain.Entities;

namespace BloomersCarriersIntegrations.Jadlog.Infrastructure.Repositorys
{
    public interface IJadlogRepository
    {
        public Task GenerateResponseLog(string pedido, string remetenteId, string response);
        public Task GenerateRequestLog(string pedido, string request);
        public Task<Order> GetInvoicedOrder(string orderNumber);
        public Task<List<Order>> GetInvoicedOrders();
        public Task<Order> GetInvoicedOrderETUR(string orderNumber);
        public Task<Parameters> GetParameters(string doc_company);
    }
}

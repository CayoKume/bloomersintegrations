using BloomersCarriersIntegrations.TotalExpress.Domain.Entities;

namespace BloomersCarriersIntegrations.TotalExpress.Infrastructure.Repositorys
{
    public interface ITotalExpressRepository
    {
        public Task GeraResponseLog(string pedido, string remetenteId, string response);
        public Task GeraRequestLog(string pedido, string request);
        public Task<Order> GetInvoicedOrder(string orderNumber);
        public Task<Order> GetInvoicedOrderETUR(string orderNumber);
        public Task<List<Order>> GetInvoicedOrders();
        public Task<IEnumerable<TotalExpressRegistroLog>> GetPedidosEnviados();
        public Task UpdatePedidoETUR(string nr_pedido);
        public Task Update_NB_DATA_COLETA(string data, string nr_pedido);
        public Task Update_NB_PREVISAO_REAL_ENTREGA(string data, string nr_pedido);
        public Task Update_NB_DATA_ENTREGA_REALIZADA(string data, string nr_pedido);
        public Task Update_NB_DATA_ULTIMO_STATUS(string data, string eventoId, string evento, string nr_pedido);
    }
}

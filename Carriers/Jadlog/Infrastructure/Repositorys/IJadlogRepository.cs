using BloomersCarriersIntegrations.Jadlog.Domain.Entities;

namespace BloomersCarriersIntegrations.Jadlog.Infrastructure.Repositorys
{
    public interface IJadlogRepository
    {
        public Task GenerateResponseLog(string pedido, Response response);
        public Task GenerateRequestLog(string pedido, string request);
        public Task<Order> GetInvoicedOrder(string orderNumber);
        public Task<List<Order>> GetInvoicedOrders();
        //public Task<Order> GetInvoicedOrderETUR(string orderNumber);
        public Task<Parameters> GetParameters(string doc_company, string tipo_servico);
        //public Task UpdatePedidoETUR(string nr_pedido);
        //public Task Update_NB_DATA_COLETA(string data, string nr_pedido);
        //public Task Update_NB_PREVISAO_REAL_ENTREGA(string data, string nr_pedido);
        //public Task Update_NB_DATA_ENTREGA_REALIZADA(string data, string nr_pedido);
        //public Task Update_NB_DATA_ULTIMO_STATUS(string data, string eventoId, string evento, string nr_pedido);
    }
}

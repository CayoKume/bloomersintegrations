using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface IDeliveryListRepository
    {
        public Task<IEnumerable<Order>?> GetOrdersShipped(string cod_transportadora, string cnpj_emp, string serie_pedido, string data_inicial, string data_final);
        public Task<Order?> GetOrderShipped(string nr_pedido, string serie, string cnpj_emp, string transportadora);
    }
}

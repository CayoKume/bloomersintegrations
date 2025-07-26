using BloomersIntegrationsCore.Domain.Entities;
using MiniWms.Domain.Entities.DeliveryList;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface IDeliveryListRepository
    {
        public Task<bool?> InsertPickedsDates(Guid guid, string deliveryListName, string carrier, IEnumerable<Order> orders);
        public Task<IEnumerable<Order>?> GetOrdersShipped(string cod_transportadora, string cnpj_emp, string serie_pedido, string data_inicial, string data_final);
        public Task<Order?> GetOrderShipped(string nr_pedido, string serie, string cnpj_emp, string transportadora);
        public Task<IEnumerable<DeliveryList>> GetDeliveryLists(string cod_transportadora, string cnpj_emp, string data_inicial, string data_final);
    }
}

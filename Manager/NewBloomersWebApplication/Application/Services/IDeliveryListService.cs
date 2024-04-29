using NewBloomersWebApplication.Domain.Entities.DeliveryList;

namespace NewBloomersWebApplication.Application.Services
{
    public interface IDeliveryListService
    {
        public Task<List<Order>?> GetOrdersShipped(string cod_transportadora, string cnpj_emp, string serie_pedido, string data_inicial, string data_final);
        public Task<Order?> GetOrderShipped(string nr_pedido, string serie, string cnpj_emp, string transportadora);
        public Task PrintOrder(List<Order> pedidos);
    }
}

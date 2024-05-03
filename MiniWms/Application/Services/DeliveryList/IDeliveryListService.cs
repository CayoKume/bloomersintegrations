namespace BloomersMiniWmsIntegrations.Application.Services
{
    public interface IDeliveryListService
    {
        public Task<string> GetOrdersShipped(string cod_transportadora, string cnpj_emp, string serie_pedido, string data_inicial, string data_final);
        public Task<string> GetOrderShipped(string nr_pedido, string serie, string cnpj_emp, string transportadora);
        public Task<bool> PrintOrder(string serializePedidosList);
        public Task<string> GetDeliveryListToPrint(string fileName);
    }
}

using BloomersMiniWmsIntegrations.Domain.Entities.Picking;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface IPickingRepository
    {
        public Task<List<Order>?> GetUnpickedOrders(string cnpj_emp, string serie_pedido, string data_inicial, string data_final);
        public Task<Order?> GetUnpickedOrder(string cnpj_emp, string serie, string nr_pedido);
        public Task<int> UpdateRetorno(string nr_pedido, int volumes, string listProdutos);
        public Task<int> UpdateShippingCompany(string nr_pedido, int cod_transportador);
    }
}

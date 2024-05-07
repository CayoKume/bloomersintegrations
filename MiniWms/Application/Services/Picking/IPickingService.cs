using BloomersMiniWmsIntegrations.Domain.Entities.Picking;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public interface IPickingService
    {
        public Task<string> GetShippingCompanys();
        public Task<string> GetUnpickedOrders(string cnpj_emp, string serie_pedido, string data_inicial, string data_final);
        public Task<string> GetUnpickedOrder(string cnpj_emp, string serie, string nr_pedido);
        public Task<bool> UpdateRetorno(string nr_pedido, int volumes, string listProdutos);
        public Task<bool> UpdateShippingCompany(string nr_pedido, int cod_transportador);
    }
}

using BloomersIntegrationsCore.Domain.Entities;
using Order = NewBloomersWebApplication.Domain.Entities.Picking.Order;

namespace NewBloomersWebApplication.Application.Services
{
    public interface IPickingService
    {
        public Task<List<ShippingCompany>?> GetShippingCompanys();
        public Task<List<Order>?> GetUnpickedOrders(string cnpj_emp, string serie_pedido, string data_inicial, string data_final);
        public Task<Order?> GetUnpickedOrder(string cnpj_emp, string serie, string nr_pedido);

        public Task<string> GetCouponToPrint(string fileName);
        public Task PrintCoupons(Order pedido);
        public Task PrintCoupon(string cnpj_emp, string serie, string nr_pedido);

        public Task<bool> UpdateRetorno(Order pedido);
        public Task<bool> UpdateShippingCompany(string nr_pedido, int cod_transportador);
    }
}

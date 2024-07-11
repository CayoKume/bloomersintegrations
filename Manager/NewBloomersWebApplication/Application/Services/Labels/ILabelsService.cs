using NewBloomersWebApplication.Domain.Entities.Labels;

namespace NewBloomersWebApplication.Application.Services
{
    public interface ILabelsService
    {
        public Task<IEnumerable<Order>?> GetOrders(string cnpj_emp, string serie_pedido, string? data_inicial, string? data_final);
        public Task<string> GetLabelToPrint(string fileName);
        public Task PrintLabels(Order pedido);
        public Task<Order> PrintLabel(string cnpj_emp, string serie_pedido, string nr_pedido);
        public Task<string> PrintCoupon(string cnpj_emp, string serie, string nr_pedido);
        public Task<bool> UpdateFlagPrinted(string nr_pedido);
    }
}

using BloomersMiniWmsIntegrations.Domain.Entities.Labels;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface ILabelsRepository
    {
        public Task<Order> GetOrdersToPresent(string cnpj_emp, string serie, string nr_pedido);
        public Task<IEnumerable<Order>> GetOrdersToPrint(string cnpj_emp, string serie, string data_inicial, string data_final);
        public Task<Order> GetOrderToPrint(string cnpj_emp, string serie, string nr_pedido);
        public Task<int> UpdatePrintedFlag(string nr_pedido);
    }
}

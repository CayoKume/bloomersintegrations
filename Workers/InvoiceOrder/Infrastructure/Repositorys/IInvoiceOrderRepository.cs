using BloomersWorkersCore.Domain.Entities;
using BloomersWorkers.InvoiceOrder.Domain.Entities;

namespace BloomersWorkers.InvoiceOrder.Infrastructure.Repositorys
{
    public interface IInvoiceOrderRepository
    {
        public Task<IEnumerable<Order>> GetOrdersFromIT4(string botName);
        public Task<int> UpdateInvoiceAttemptIT4(string number, int invoice_attempt);
        public Task<MicrovixUser> GetMicrovixUser(string gabot);
    }
}

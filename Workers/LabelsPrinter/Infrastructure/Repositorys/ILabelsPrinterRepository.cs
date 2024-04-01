using BloomersWorkers.LabelsPrinter.Domain.Entities;

namespace BloomersWorkers.LabelsPrinter.Infrastructure.Repositorys
{
    public interface ILabelsPrinterRepository
    {
        public Task<IEnumerable<Order>> GetOrders();
        public Task UpdateStatus(string orderNumber);
    }
}

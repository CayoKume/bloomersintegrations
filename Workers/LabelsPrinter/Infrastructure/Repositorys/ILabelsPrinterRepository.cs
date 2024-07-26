using BloomersWorkers.LabelsPrinter.Domain.Entities;

namespace BloomersWorkers.LabelsPrinter.Infrastructure.Repositorys
{
    public interface ILabelsPrinterRepository
    {
        public Task<List<Order>> GetOrders();
        public Task<List<Order>> GetJadlogOrders();
        public Task UpdateStatus(string orderNumber);
    }
}

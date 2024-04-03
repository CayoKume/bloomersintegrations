using BloomersWorkers.ChangingOrder.Domain.Entities;
using BloomersWorkersCore.Domain.Entities;

namespace BloomersWorkers.ChangingOrder.Infrastructure.Repositorys
{
    public interface IChangingOrderRepository
    {
        public Task<List<Order>> GetOrdersFromIT4();
        public Task UpdateReturnIT4ITEM(string nr_pedido, string idControle);
        public Task<MicrovixUser> GetMicrovixUser(string gabot);
    }
}

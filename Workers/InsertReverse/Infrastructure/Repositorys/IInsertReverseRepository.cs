using BloomersWorkers.InsertReverse.Domain.Entities;

namespace BloomersWorkers.InsertReverse.Infrastructure.Repositorys
{
    public interface IInsertReverseRepository
    { 
        public Task<IEnumerable<Order>> GetReverses();
        public Task InsereLogDeSucesso(Order order);
    }
}

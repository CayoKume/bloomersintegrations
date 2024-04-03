using BloomersWorkers.InsertReverse.Domain.Entities;

namespace BloomersWorkers.InsertReverse.Infrastructure.Repositorys
{
    public class InsertReverseRepository : IInsertReverseRepository
    {
        public Task<IEnumerable<Order>> GetReverses()
        {
            throw new NotImplementedException();
        }

        public Task InsereLogDeSucesso(Order order)
        {
            throw new NotImplementedException();
        }
    }
}

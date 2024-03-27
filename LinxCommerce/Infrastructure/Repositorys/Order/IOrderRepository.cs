using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys
{
    public interface IOrderRepository
    {
        public void BulkInsertIntoTableRaw(List<Order> registros, string? database);
        public Task<int> GetParameters(string tableName);
        public Task<List<Order>> GetRegistersExists(List<string> ordersIds, string? database);
    }
}

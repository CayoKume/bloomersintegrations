using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys
{
    public interface IOrderRepository
    {
        public void BulkInsertIntoTableRaw(List<Order> registros, string? database);
        public Task<string> GetParameters(string tableName, string sql);
        public Task<List<Order>> GetRegistersExists(List<string> ordersIds, string? database);
    }
}

using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys
{
    public interface IProductRepository
    {
        public void BulkInsertIntoTableRaw(List<Product> registros, string? database);
        public Task<int> GetParameters(string tableName);
        public Task<List<Product>> GetRegistersExists(List<string> ordersIds, string? database);
    }
}

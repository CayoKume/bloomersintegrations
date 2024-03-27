using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys
{
    public interface ISKURepository
    {
        public void BulkInsertIntoTableRaw(List<SKUs> registros, string? database);
        public void BulkInsertIntoTableRaw(SearchSKUResponse.Root registros, string? database);
        public Task<string> GetParameters(string tableName, string sql);
        public Task<List<SKUs>> GetRegistersExists(List<string> ordersIds, string? database);
    }
}

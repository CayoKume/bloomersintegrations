using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using System.Data;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys
{
    public interface ISKURepository
    {
        public void BulkInsertIntoTableRaw(List<SKUs> registros, string? database);
        public void BulkInsertIntoTableRaw(SearchSKUResponse.Root registros, string? database);
        public Task<int> GetParameters(string tableName);
        public Task<List<SKUs>> GetRegistersExists(List<string> ordersIds, string? database);
    }
}

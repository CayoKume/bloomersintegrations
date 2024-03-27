using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using System.Data;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys.Base
{
    public interface ILinxCommerceRepositoryBase<TEntity> where TEntity : class, new()
    {
        public void BulkInsertIntoTableRaw(DataTable dataTable, string? database, string? tableName, int dataTableRowsNumber);
        public Task<int> GetParameters(string tableName, string sql);
        public Task<TEntity?> GetRegisterExists(string tableName, string sql);
        public Task<List<TEntity>> GetRegistersExists(string? tableName, string? sql);
        public Task IntegraRegistros(string tableName, string sql);
        public DataTable CreateDataTable(string tableName, List<string> properties);
    }
}

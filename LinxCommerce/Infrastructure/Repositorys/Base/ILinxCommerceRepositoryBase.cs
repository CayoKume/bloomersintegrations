using System.Data;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys.Base
{
    public interface ILinxCommerceRepositoryBase<TEntity> where TEntity : class, new()
    {
        public void BulkInsertIntoTableRaw(DataTable dataTable, string? database, string? tableName, int dataTableRowsNumber);
        public Task CallDbProcMerge(string? procName, string? tableName, string? database);
        public void CallDbProcMergeSync(string? procName, string? tableName, string? database);
        public Task<string> GetParameters(string tableName, string sql);
        public string GetParametersSync(string tableName, string sql);
        public Task<TEntity?> GetRegisterExists(string tableName, string sql);
        public Task<List<TEntity>> GetRegistersExists(string? tableName, string? sql);
        public Task IntegraRegistros(string tableName, string sql);
        public void IntegraRegistrosSync(string tableName, string sql);
    }
}

using BloomersIntegrationsCore.Domain.Entities;
using System.Data;
using System.Reflection;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base
{
    public interface ILinxMicrovixRepositoryBase<TEntity> where TEntity : class
    {
        public void BulkInsertIntoTableRaw(DataTable dataTable, string database, string tableName, int dataTableRowsNumber);

        public Task<IEnumerable<string>> GetProductsAsync(string tableName, string sql);
        public Task<string> GetParametersAsync(string tableName, string sql);
        public string GetParametersNotAsync(string tableName, string sql);
        public Task<string> GetTableLastTimestampAsync(string tableName, string sql);
        public string GetTableLastTimestampNotAsync(string tableName, string sql);
        public Task<TEntity?> GetRegisterExistsAsync(string tableName, string sql);
        public TEntity? GetRegisterExistsNotAsync(string tableName, string sql);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string sql);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string sql);
        public Task<List<TEntity>> GetRegistersExistsAsync(string tableName, string sql);
        public List<TEntity> GetRegistersExistsNotAsync(string tableName, string sql);
        public Task<IEnumerable<String>> GetCodDepositosAsync(string tableName, string sql);
        public IEnumerable<String> GetCodDepositosNotAsync(string tableName, string sql);
        public Task<IEnumerable<String>> GetIdTabelaPrecoAsync(string tableName, string sql);
        public IEnumerable<String> GetIdTabelaPrecoNotAsync(string tableName, string sql);

        public Task InsereRegistroIndividualAsync(string tableName, string sql, object registro);
        public void InsereRegistroIndividualNotAsync(string tableName, string sql, object registro);
        
        public Task CallDbProcMergeAsync(string procName, string tableName, string database);
        public void CallDbProcMergeNotAsync(string procName, string tableName, string database);
        
        public DataTable CreateDataTable(string tableName, PropertyInfo[] properties);
    }
}

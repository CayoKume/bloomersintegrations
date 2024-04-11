using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosInventarioRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxProdutosInventario> registros, string tableName, string database);
        public Task<List<LinxProdutosInventario>> GetRegistersExistsAsync(List<LinxProdutosInventario> registros, string tableName, string database);
        public List<LinxProdutosInventario> GetRegistersExistsNotAsync(List<LinxProdutosInventario> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task<IEnumerable<String>> GetCodDepositosAsync(string tableName);
        public IEnumerable<String> GetCodDepositosNotAsync(string tableName);
        public Task InsereRegistroIndividualAsync(LinxProdutosInventario registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxProdutosInventario registro, string tableName, string database);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
        public Task CallDbProcMergeAsync(string procName, string tableName, string database);
        public void CallDbProcMergeNotAsync(string procName, string tableName, string database);
    }
}

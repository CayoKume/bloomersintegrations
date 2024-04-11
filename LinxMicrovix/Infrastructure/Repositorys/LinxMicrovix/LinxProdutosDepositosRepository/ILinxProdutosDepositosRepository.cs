using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosDepositosRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxProdutosDepositos> registros, string tableName, string database);
        public Task<List<LinxProdutosDepositos>> GetRegistersExistsAsync(List<LinxProdutosDepositos> registros, string tableName, string database);
        public List<LinxProdutosDepositos> GetRegistersExistsNotAsync(List<LinxProdutosDepositos> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxProdutosDepositos registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxProdutosDepositos registro, string tableName, string database);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
        public Task CallDbProcMergeAsync(string procName, string tableName, string database);
        public void CallDbProcMergeNotAsync(string procName, string tableName, string database);
    }
}

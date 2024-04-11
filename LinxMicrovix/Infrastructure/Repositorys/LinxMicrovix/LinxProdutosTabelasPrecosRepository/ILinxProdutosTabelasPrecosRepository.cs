using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosTabelasPrecosRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxProdutosTabelasPrecos> registros, string tableName, string database);
        public Task<List<LinxProdutosTabelasPrecos>> GetRegistersExistsAsync(List<LinxProdutosTabelasPrecos> registros, string tableName, string database);
        public List<LinxProdutosTabelasPrecos> GetRegistersExistsNotAsync(List<LinxProdutosTabelasPrecos> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxProdutosTabelasPrecos registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxProdutosTabelasPrecos registro, string tableName, string database);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
        public Task<IEnumerable<String>> GetIdTabelaPrecoAsync(string cnpj, string tableName, string database);
        public IEnumerable<String> GetIdTabelaPrecoNotAsync(string cnpj, string tableName, string database);
        public Task CallDbProcMergeAsync(string procName, string tableName, string database);
        public void CallDbProcMergeNotAsync(string procName, string tableName, string database);
    }
}

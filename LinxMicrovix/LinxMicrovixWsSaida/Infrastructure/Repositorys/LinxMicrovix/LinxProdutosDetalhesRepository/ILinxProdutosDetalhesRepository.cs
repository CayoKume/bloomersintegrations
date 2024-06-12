using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosDetalhesRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxProdutosDetalhes> registros, string tableName, string database);
        public Task<List<LinxProdutosDetalhes>> GetRegistersExistsAsync(List<LinxProdutosDetalhes> registros, string tableName, string database);
        public List<LinxProdutosDetalhes> GetRegistersExistsNotAsync(List<LinxProdutosDetalhes> registros, string tableName, string database);
        public Task<IEnumerable<string>> GetProductsAsync(string tableName, string database, Company company);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxProdutosDetalhes registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxProdutosDetalhes registro, string tableName, string database);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
        public Task CallDbProcMergeAsync(string procName, string tableName, string database);
        public void CallDbProcMergeNotAsync(string procName, string tableName, string database);
    }
}

using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosTabelasRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxProdutosTabelas> registros, string tableName, string database);
        public Task<List<LinxProdutosTabelas>> GetRegistersExistsAsync(List<LinxProdutosTabelas> registros, string tableName, string database);
        public List<LinxProdutosTabelas> GetRegistersExistsNotAsync(List<LinxProdutosTabelas> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxProdutosTabelas registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxProdutosTabelas registro, string tableName, string database);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
        public Task CallDbProcMergeAsync(string procName, string tableName, string database);
        public void CallDbProcMergeNotAsync(string procName, string tableName, string database);
    }
}

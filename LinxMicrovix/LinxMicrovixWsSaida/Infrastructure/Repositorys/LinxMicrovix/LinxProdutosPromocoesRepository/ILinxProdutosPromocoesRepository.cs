using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosPromocoesRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxProdutosPromocoes> registros, string tableName, string database);
        public Task<List<LinxProdutosPromocoes>> GetRegistersExistsAsync(List<LinxProdutosPromocoes> registros, string tableName, string database);
        public List<LinxProdutosPromocoes> GetRegistersExistsNotAsync(List<LinxProdutosPromocoes> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
        public Task CallDbProcMergeAsync(string procName, string tableName, string database);
        public void CallDbProcMergeNotAsync(string procName, string tableName, string database);
    }
}

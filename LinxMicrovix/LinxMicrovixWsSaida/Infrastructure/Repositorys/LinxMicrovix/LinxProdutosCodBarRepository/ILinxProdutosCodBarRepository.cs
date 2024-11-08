using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosCodBarRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxProdutosCodBar> registros, string tableName, string database);
        public Task<List<LinxProdutosCodBar>> GetRegistersExistsAsync(List<LinxProdutosCodBar> registros, string tableName, string database);
        public List<LinxProdutosCodBar> GetRegistersExistsNotAsync(List<LinxProdutosCodBar> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxProdutosCodBar registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxProdutosCodBar registro, string tableName, string database);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
    }
}

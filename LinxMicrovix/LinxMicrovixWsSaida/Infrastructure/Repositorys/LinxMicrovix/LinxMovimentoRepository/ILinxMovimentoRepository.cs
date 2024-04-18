using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxMovimentoRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxMovimento> registros, string tableName, string database);
        public Task<List<LinxMovimento>> GetRegistersExistsAsync(List<LinxMovimento> registros, string tableName, string database);
        public List<LinxMovimento> GetRegistersExistsNotAsync(List<LinxMovimento> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxMovimento registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxMovimento registro, string tableName, string database);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
    }
}

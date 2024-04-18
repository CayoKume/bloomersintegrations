using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxMovimentoCartoesRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxMovimentoCartoes> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
        public Task<List<LinxMovimentoCartoes>> GetRegistersExistsAsync(List<LinxMovimentoCartoes> registros, string tableName, string database);
        public List<LinxMovimentoCartoes> GetRegistersExistsNotAsync(List<LinxMovimentoCartoes> registros, string tableName, string database);
    }
}

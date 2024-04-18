using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxLojasRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxLojas> registros, string tableName, string database);
        public Task<List<LinxLojas>> GetRegistersExistsAsync(List<LinxLojas> registros, string tableName, string database);
        public List<LinxLojas> GetRegistersExistsNotAsync(List<LinxLojas> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxLojas registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxLojas registro, string tableName, string database);
    }
}

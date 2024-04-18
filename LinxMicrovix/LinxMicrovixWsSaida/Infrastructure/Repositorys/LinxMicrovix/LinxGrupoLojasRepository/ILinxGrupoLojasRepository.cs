using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxGrupoLojasRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxGrupoLojas> registros, string tableName, string database);
        public Task<List<LinxGrupoLojas>> GetRegistersExistsAsync(List<LinxGrupoLojas> registros, string tableName, string database);
        public List<LinxGrupoLojas> GetRegistersExistsNotAsync(List<LinxGrupoLojas> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
    }
}

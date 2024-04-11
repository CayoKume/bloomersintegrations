using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxPlanosRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxPlanos> registros, string tableName, string database);
        public Task<List<LinxPlanos>> GetRegistersExistsAsync(List<LinxPlanos> registros, string tableName, string database);
        public List<LinxPlanos> GetRegistersExistsNotAsync(List<LinxPlanos> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxPlanos registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxPlanos registro, string tableName, string database);
    }
}

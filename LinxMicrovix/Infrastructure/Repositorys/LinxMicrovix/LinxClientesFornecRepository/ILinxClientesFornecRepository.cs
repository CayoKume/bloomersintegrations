using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxClientesFornecRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxClientesFornec> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task<List<LinxClientesFornec>> GetRegistersExistsAsync(List<LinxClientesFornec> registros, string tableName, string database);
        public List<LinxClientesFornec> GetRegistersExistsNotAsync(List<LinxClientesFornec> registros, string tableName, string database);
        public Task InsereRegistroIndividualAsync(LinxClientesFornec registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxClientesFornec registro, string tableName, string database);
    }
}

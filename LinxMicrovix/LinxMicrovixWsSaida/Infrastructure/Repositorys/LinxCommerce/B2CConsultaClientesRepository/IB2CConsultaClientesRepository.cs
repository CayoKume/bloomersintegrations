using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce
{
    public interface IB2CConsultaClientesRepository
    {
        public void BulkInsertIntoTableRaw(List<B2CConsultaClientes> registros, string tableName, string database);
        public Task<string> GetTableLastTimestampAsync(string database, string tableName);
        public string GetTableLastTimestampNotAsync(string database, string tableName);
        public Task<List<B2CConsultaClientes>> GetRegistersExistsAsync(List<B2CConsultaClientes> registros, string tableName, string database);
        public List<B2CConsultaClientes> GetRegistersExistsNotAsync(List<B2CConsultaClientes> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(B2CConsultaClientes registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(B2CConsultaClientes registro, string tableName, string database);
    }
}

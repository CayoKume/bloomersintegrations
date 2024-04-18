using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce
{
    public interface IB2CConsultaStatusRepository
    {
        public void BulkInsertIntoTableRaw(List<B2CConsultaStatus> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task<List<B2CConsultaStatus>> GetRegistersExistsAsync(List<B2CConsultaStatus> registros, string tableName, string database);
        public List<B2CConsultaStatus> GetRegistersExistsNotAsync(List<B2CConsultaStatus> registros, string tableName, string database);
    }
}

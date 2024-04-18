using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce
{
    public interface IB2CConsultaPedidosStatusRepository
    {
        public void BulkInsertIntoTableRaw(List<B2CConsultaPedidosStatus> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task<List<B2CConsultaPedidosStatus>> GetRegistersExistsAsync(List<B2CConsultaPedidosStatus> registros, string tableName, string database);
        public List<B2CConsultaPedidosStatus> GetRegistersExistsNotAsync(List<B2CConsultaPedidosStatus> registros, string tableName, string database);
        public Task InsereRegistroIndividualAsync(B2CConsultaPedidosStatus registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(B2CConsultaPedidosStatus registro, string tableName, string database);
    }
}

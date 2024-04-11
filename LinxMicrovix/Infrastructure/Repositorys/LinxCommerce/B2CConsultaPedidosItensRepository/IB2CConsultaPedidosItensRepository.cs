using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxCommerce
{
    public interface IB2CConsultaPedidosItensRepository
    {
        public void BulkInsertIntoTableRaw(List<B2CConsultaPedidosItens> registros, string tableName, string database);
        public Task<string> GetTableLastTimestampAsync(string database, string tableName);
        public string GetTableLastTimestampNotAsync(string database, string tableName);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task<List<B2CConsultaPedidosItens>> GetRegistersExistsAsync(List<B2CConsultaPedidosItens> registros, string tableName, string database);
        public List<B2CConsultaPedidosItens> GetRegistersExistsNotAsync(List<B2CConsultaPedidosItens> registros, string tableName, string database);
        public Task InsereRegistroIndividualAsync(B2CConsultaPedidosItens registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(B2CConsultaPedidosItens registro, string tableName, string database);
    }
}

using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxCommerce
{
    public interface IB2CConsultaPedidosRepository
    {
        public void BulkInsertIntoTableRaw(List<B2CConsultaPedidos> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task<List<B2CConsultaPedidos>> GetRegistersExistsAsync(List<B2CConsultaPedidos> registros, string tableName, string database);
        public List<B2CConsultaPedidos> GetRegistersExistsNotAsync(List<B2CConsultaPedidos> registros, string tableName, string database);
        public Task InsereRegistroIndividualAsync(B2CConsultaPedidos registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(B2CConsultaPedidos registro, string tableName, string database);
    }
}

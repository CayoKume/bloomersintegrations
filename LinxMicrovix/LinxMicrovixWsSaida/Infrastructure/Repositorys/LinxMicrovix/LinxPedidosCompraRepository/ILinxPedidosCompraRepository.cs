using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxPedidosCompraRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxPedidosCompra> registros, string tableName, string database);
        public Task<List<LinxPedidosCompra>> GetRegistersExistsAsync(List<LinxPedidosCompra> registros, string tableName, string database);
        public List<LinxPedidosCompra> GetRegistersExistsNotAsync(List<LinxPedidosCompra> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
    }
}

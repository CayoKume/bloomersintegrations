using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxPedidosVendaRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxPedidosVenda> registros, string tableName, string database);
        public Task<List<LinxPedidosVenda>> GetRegistersExistsAsync(List<LinxPedidosVenda> registros, string tableName, string database);
        public List<LinxPedidosVenda> GetRegistersExistsNotAsync(List<LinxPedidosVenda> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxPedidosVenda registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxPedidosVenda registro, string tableName, string database);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
    }
}

using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ISQLServerConnection _conn;

        public HomeRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<IEnumerable<Order>?> GetPickupOrders(string doc_company)
        {
            var sql = $@"SELECT DOCUMENTO AS NUMBER FROM GENERAL..IT4_WMS_DOCUMENTO (NOLOCK) WHERE NB_TRANSPORTADORA = 65281 AND NB_DOC_REMETENTE = {doc_company} AND CHAVE_NFE IS NULL AND CANCELADO IS NULL AND CANCELAMENTO IS NULL";

            try
            {
                return await _conn.GetDbConnection().QueryAsync<Order>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Home] - GetPickupOrders - Erro ao obter pedidos na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }
    }
}

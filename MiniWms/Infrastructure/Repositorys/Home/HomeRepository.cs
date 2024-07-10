using BloomersMiniWmsIntegrations.Domain.Entities.Home;
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
            var sql = $@"SELECT DOCUMENTO AS NUMBER, DATA AS DATA_PEDIDO FROM GENERAL..IT4_WMS_DOCUMENTO (NOLOCK) WHERE (NB_TRANSPORTADORA = 65281 OR NB_TRANSPORTADORA = 97586) AND NB_DOC_REMETENTE = {doc_company} AND CHAVE_NFE IS NULL AND CANCELADO IS NULL AND CANCELAMENTO IS NULL AND DATA > '2024-06-01'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.QueryAsync<Order>(sql); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Home] - GetPickupOrders - Erro ao obter pedidos na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }
    }
}

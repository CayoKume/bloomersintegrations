using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersMiniWmsIntegrations.Domain.Entities.ExecuteCancellation;
using Dapper;
using Order = BloomersMiniWmsIntegrations.Domain.Entities.ExecuteCancellation.Order;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public class ExecuteCancellationRepository : IExecuteCancellationRepository
    {
        private readonly ISQLServerConnection _conn;

        public ExecuteCancellationRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<IEnumerable<Order>> GetOrdersToCancel(string serie, string doc_company)
        {
            var sql = $@"SELECT DISTINCT
                         B.PEDIDO AS NUMBER,
                         IIF(B.DATA_CANCELAMENTO IS NULL, 'CANCELAR', 'CANCELADO') AS CANCELED,
                         D.ID_MOTIVO AS ID_MOTIVO,
                         D.DESCRICAO_MOTIVO AS DESCRICAO_MOTIVO,

                         A.NB_CODIGO_CLIENTE AS COD_CLIENT,
                         A.NB_RAZAO_CLIENTE AS REASON_CLIENT,
                         A.NB_NOME_CLIENTE AS NAME_CLIENT,
                         A.NB_DOC_CLIENTE AS DOC_CLIENT,
                         A.NB_EMAIL_CLIENTE AS EMAIL_CLIENT,
                         A.NB_ENDERECO_CLIENTE AS ADDRESS_CLIENT,
                         A.NB_NUMERO_RUA_CLIENTE AS STREET_NUMBER_CLIENT,
                         A.NB_COMPLEMENTO_END_CLIENTE AS COMPLEMENT_ADDRESS_CLIENT,
                         A.NB_BAIRRO_CLIENTE AS NEIGHBORHOOD_CLIENT,
                         A.NB_CIDADE AS CITY_CLIENT,
                         A.NB_ESTADO AS UF_CLIENT,
                         TRIM(REPLACE(A.NB_CEP, '-', '')) AS ZIP_CODE_CLIENT,
                         A.NB_FONE_CLIENTE AS FONE_CLIENT,
                         A.NB_INSCRICAO_ESTADUAL_CLIENTE AS STATE_REGISTRATION_CLIENT,
                         A.NB_INSCRICAO_MUNICIPAL_CLIENTE AS MUNICIPAL_REGISTRATION_CLIENT,
                         
                         C.COD_PRODUTO AS COD_PRODUCT,
                         C.DESCRICAO AS DESCRIPTION_PRODUCT,
                         C.QTDE AS QUANTITY_PRODUCT,
                         C.QTDERETORNO AS PICKED_QUANTITY,
                         C.VALOR_UNITARIO_PRODUTO AS UNITARY_VALUE_PRODUCT,
                         C.VALOR_TOTAL_PRODUTO AS AMOUNT_PRODUCT
                         
                         FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK) 
                         JOIN GENERAL..TB_NB_CANCELAMENTO_PEDIDOS B (NOLOCK) ON A.DOCUMENTO = B.PEDIDO
                         JOIN GENERAL..TB_NB_CANCELAMENTO_PEDIDOS_ITENS C (NOLOCK) ON B.ID_CANCELAMENTO_PEDIDO = C.ID_CANCELAMENTO_PEDIDO
                         JOIN GENERAL..TB_NB_CANCELAMENTO_PEDIDOS_MOTIVOS D (NOLOCK) ON D.ID_MOTIVO = B.MOTIVO_REGISTRO

                         WHERE
                         B.SUPORTE IS NULL
                         AND B.DATA_CANCELAMENTO IS NULL
                         AND B.MOTIVO_CANCELAMENTO IS NULL
                         AND A.SERIE = '{serie}'
                         AND A.NB_DOC_REMETENTE = '{doc_company}'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Client, ProductToExecuteCancellation, Order>(sql, (pedido, cliente, produto) =>
                    {
                        pedido.client = cliente;
                        pedido.itens.Add(produto);
                        return pedido;
                    }, splitOn: "cod_client, cod_product");

                    var pedidos = result.GroupBy(p => p.number).Select(g =>
                    {
                        var groupedOrder = g.First();
                        groupedOrder.itens = g.Select(p => p.itens.Single()).ToList();
                        return groupedOrder;
                    });

                    return pedidos;
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"MiniWms [ExecuteCancellation] - GetOrdersToCancel - Erro ao obter pedido na tabela TB_NB_CANCELAMENTO_PEDIDOS  - {ex.Message}");
            }
        }

        public async Task<Dictionary<int, string>> GetReasons()
        {
            var sql = $@"SELECT ID_MOTIVO AS [KEY], DESCRICAO_MOTIVO AS [VALUE] FROM GENERAL..TB_NB_CANCELAMENTO_PEDIDOS_MOTIVOS";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<KeyValuePair<int, string>>(sql);
                    return result.ToDictionary(pair => pair.Key, pair => pair.Value);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [ExecuteCancellation] - GetReasons - Erro ao obter motivos dos cancelamentos - {ex.Message}");
            }
        }

        public async Task<bool> UpdateDateCanceled(string number, string suporte, string inputObs, int motivo)
        {
            var sql = $@"UPDATE [GENERAL].[dbo].[TB_NB_CANCELAMENTO_PEDIDOS] SET SUPORTE = '{suporte}', DATA_CANCELAMENTO = GETDATE(), OBS_SUPORTE = '{inputObs}', MOTIVO_CANCELAMENTO = {motivo} WHERE PEDIDO = '{number}'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.ExecuteAsync(sql);

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [ExecuteCancellation] - UpdateDateCanceled - Erro ao atualizar data de cancelamento do pedido {number} na tabela TB_NB_CANCELAMENTO_PEDIDOS  - {ex.Message}");
            }
        }
    }
}

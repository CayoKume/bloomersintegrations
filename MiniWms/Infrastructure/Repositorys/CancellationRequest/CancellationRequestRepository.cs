using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersMiniWmsIntegrations.Domain.Entities.CancellationRequest;
using Dapper;
using System.Drawing;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public class CancellationRequestRepository : ICancellationRequestRepository
    {
        private readonly ISQLServerConnection _conn;

        public CancellationRequestRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<bool> CreateCancellationRequest(Order order)
        {
            var stringItens = String.Empty;

            foreach (var item in order.itens) 
            {
                stringItens += $@"(@ID_CANCELAMENTO_PEDIDO, '{order.number}', {item.cod_product}, '{item.description_product}', {item.quantity_product}, {item.picked_quantity_product}, {item.unitary_value_product}, {item.amount_product})";
            }

            var sql = $@"BEGIN TRANSACTION;

                            BEGIN TRY
                                INSERT INTO GENERAL..TB_NB_CANCELAMENTO_PEDIDOS (DATA_REGISTRO, VENDEDORA, MOTIVO_REGISTRO, PEDIDO)
                                VALUES (GETDATE(), '{order.requester}', {order.reason}, '{order.number}');
                             
                                DECLARE @ID_CANCELAMENTO_PEDIDO BIGINT;
                                SET @ID_CANCELAMENTO_PEDIDO = SCOPE_IDENTITY();
                             
                                INSERT INTO GENERAL..TB_NB_CANCELAMENTO_PEDIDOS_ITENS (ID_CANCELAMENTO_PEDIDO, PEDIDO, COD_PRODUTO, DESCRICAO, QTDE, QTDERETORNO, VALOR_UNITARIO_PRODUTO, VALOR_TOTAL_PRODUTO)
                                VALUES 
                                [0];
                             
                                COMMIT TRANSACTION;
                            END TRY
                            BEGIN CATCH
                                ROLLBACK TRANSACTION;
                            END CATCH;";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.ExecuteAsync(sql.Replace("[0]", stringItens));
                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [CancellationRequest] - CreateCancellationRequest - Erro ao criar solicitação de cancelamento do pedido: {order.number}  - {ex.Message}");
            }
        }

        public async Task<Order> GetOrderToCancel(string number, string serie, string doc_company)
        {
            var sql = $@"SELECT 
                        DOCUMENTO AS NUMBER, 
                        CODIGO_BARRA AS COD_PRODUCT, 
                        DESCRICAO AS DESCRIPTION_PRODUCT, 
                        QTDE AS QUANTITY_PRODUCT, 
                        QTDERETORNO AS PICKED_QUANTITY_PRODUCT, 
                        NB_SKU_PRODUTO AS SKU_PRODUCT,
                        NB_VALOR_UNITARIO_PRODUTO AS UNITARY_VALUE_PRODUCT,
                        NB_VALOR_TOTAL_PRODUTO AS AMOUNT_PRODUCT,
                        NB_VALOR_FRETE_PRODUTO AS SHIPPING_VALUE_PRODUCT

                        FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                        JOIN GENERAL..IT4_WMS_DOCUMENTO_ITEM B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE
                        WHERE
                        DOCUMENTO IN ('{number}')
                        AND SERIE = '{serie}'
                        AND NB_DOC_REMETENTE = '{doc_company}'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<Order, ProductToCancellation, Order>(sql, (pedido, produto) =>
                    {
                        pedido.itens.Add(produto);
                        return pedido;
                    }, splitOn: "cod_product");

                    var pedido = result.GroupBy(p => p.number).Select(g =>
                    {
                        var groupedOrder = g.First();
                        groupedOrder.itens = g.Select(p => p.itens.Single()).ToList();
                        return groupedOrder;
                    });

                    return pedido.First();
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"MiniWms [CancellationRequest] - GetOrder - Erro ao obter pedido na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
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
                throw new Exception($"MiniWms [CancellationRequest] - GetReasons - Erro ao obter motivos dos cancelamentos - {ex.Message}");
            }
        }
    }
}

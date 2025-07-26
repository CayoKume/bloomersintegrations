using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;
using MiniWms.Domain.Entities.DeliveryList;
using System.Data;
using System.Threading;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public class DeliveryListRepository : IDeliveryListRepository
    {
        private readonly ISQLServerConnection _conn;

        public DeliveryListRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<Order?> GetOrderShipped(string nr_pedido, string serie, string cnpj_emp, string transportadora)
        {
            var sql = $@"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) AS NUMBER,
                        A.VOLUMES AS VOLUMES,
                                                            
                        A.NB_CODIGO_CLIENTE AS COD_CLIENT,
                        A.NB_RAZAO_CLIENTE AS REASON_CLIENT,
                        A.NB_DOC_CLIENTE AS DOC_CLIENT,
                        A.NB_ENDERECO_CLIENTE AS ADDRESS_CLIENT,
                        A.NB_NUMERO_RUA_CLIENTE AS STREET_NUMBER_CLIENT,
                        A.NB_COMPLEMENTO_END_CLIENTE AS COMPLEMENT_ADDRESS_CLIENT,
                        A.NB_BAIRRO_CLIENTE AS NEIGHBORHOOD_CLIENT,
                        A.NB_CIDADE AS CITY_CLIENT,
                        A.NB_ESTADO AS UF_CLIENT,
                        TRIM(REPLACE(A.NB_CEP, '-', '')) AS ZIP_CODE_CLIENT,
                        A.NB_FONE_CLIENTE AS FONE_CLIENT,
                        
                        A.NB_TRANSPORTADORA AS COD_SHIPPINGCOMPANY,
                        A.NB_RAZAO_TRANSPORTADORA AS REASON_SHIPPINGCOMPANY,

                        A.NF_SAIDA AS NUMBER_NF,
                        A.NB_VALOR_PEDIDO AS AMOUNT_NF,

                        A.NB_DOC_REMETENTE AS DOC_COMPANY

                        FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                        WHERE
                        A.SERIE = '{serie}'
                        AND A.NB_DOC_REMETENTE = '{cnpj_emp}' 
						AND A.DOCUMENTO = '{nr_pedido}'
                        AND A.CHAVE_NFE IS NOT NULL
                        AND A.XML_FATURAMENTO IS NOT NULL
                        AND A.NF_SAIDA IS NOT NULL
                        AND A.NB_TRANSPORTADORA = '{transportadora}'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Client, ShippingCompany, Invoice, Company, Order>(sql, (pedido, cliente, transportadora, notaFiscal, empresa) =>
                    {
                        pedido.client = cliente;
                        pedido.shippingCompany = transportadora;
                        pedido.invoice = notaFiscal;
                        pedido.company = empresa;
                        return pedido;
                    }, splitOn: "cod_client, cod_shippingCompany, number_nf, doc_company");

                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [DeliveryList] - GetOrderShipped - Erro ao obter pedido: {nr_pedido} na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<IEnumerable<Order>?> GetOrdersShipped(string cod_transportadora, string cnpj_emp, string serie_pedido, string data_inicial, string data_final)
        {
            var sql = @"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) AS NUMBER,
                        A.VOLUMES AS VOLUMES,

						A.NB_CODIGO_CLIENTE AS COD_CLIENT,
                        A.NB_RAZAO_CLIENTE AS REASON_CLIENT,
                        A.NB_DOC_CLIENTE AS DOC_CLIENT,
                        A.NB_ENDERECO_CLIENTE AS ADDRESS_CLIENT,
                        A.NB_NUMERO_RUA_CLIENTE AS STREET_NUMBER_CLIENT,
                        A.NB_COMPLEMENTO_END_CLIENTE AS COMPLEMENT_ADDRESS_CLIENT,
                        A.NB_BAIRRO_CLIENTE AS NEIGHBORHOOD_CLIENT,
                        A.NB_CIDADE AS CITY_CLIENT,
                        A.NB_ESTADO AS UF_CLIENT,
                        TRIM(REPLACE(A.NB_CEP, '-', '')) AS ZIP_CODE_CLIENT,
                        A.NB_FONE_CLIENTE AS FONE_CLIENT,

                        A.NB_TRANSPORTADORA AS COD_SHIPPINGCOMPANY,
                        A.NB_RAZAO_TRANSPORTADORA AS REASON_SHIPPINGCOMPANY,

                        A.NF_SAIDA AS NUMBER_NF,
                        A.NB_VALOR_PEDIDO AS AMOUNT_NF,

                        A.NB_DOC_REMETENTE AS DOC_COMPANY

                        [0]";

            if (cod_transportadora == "7601")
            {
                sql = sql.Replace("[0]", $@"FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                                            JOIN GENERAL..TOTALEXPRESSREGISTROLOG B (NOLOCK) ON A.DOCUMENTO = B.PEDIDO
                                            WHERE
                                            A.SERIE = '{serie_pedido}'
                                            AND A.NB_DOC_REMETENTE = '{cnpj_emp}'
                                            AND A.CHAVE_NFE IS NOT NULL
                                            AND A.XML_FATURAMENTO IS NOT NULL
                                            AND A.NF_SAIDA IS NOT NULL
                                            AND A.NB_TRANSPORTADORA = '{cod_transportadora}'
                                            AND B.DATAENVIO >= CONVERT(DATE, '{data_inicial.Trim()}')
                                            AND B.DATAENVIO <= CONCAT (CONVERT(DATE, '{data_final.Trim()}'),' 23:59:59')");
            }
            else
            {
                sql = sql.Replace("[0]", $@"FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                                            WHERE
                                            A.SERIE = '{serie_pedido}'
                                            AND A.NB_DOC_REMETENTE = '{cnpj_emp}'
                                            AND A.CHAVE_NFE IS NOT NULL
                                            AND A.XML_FATURAMENTO IS NOT NULL
                                            AND A.NF_SAIDA IS NOT NULL
                                            AND A.NB_TRANSPORTADORA = '{cod_transportadora}'
                                            AND A.RETORNO > '{data_inicial}'
                                            AND A.RETORNO < '{data_final}'");
            }

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.QueryAsync<Order, Client, ShippingCompany, Invoice, Company, Order>(sql, (pedido, cliente, transportadora, notaFiscal, empresa) =>
                            {
                                pedido.client = cliente;
                                pedido.shippingCompany = transportadora;
                                pedido.invoice = notaFiscal;
                                pedido.company = empresa;
                                return pedido;
                            }, splitOn: "cod_client, cod_shippingCompany, number_nf, doc_company");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Imprime Romaneio] - GetPedidosEnviados - Erro ao obter pedidos na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<IEnumerable<DeliveryList>> GetDeliveryLists(string cod_transportadora, string cnpj_emp, string data_inicial, string data_final)
        {
            var sql = $@"SELECT DISTINCT
                        A.[uniqueidentifier] as identificador,
                        A.[name] as deliveryListName,
                        A.[carrier] as transportadora

                        FROM azure.newbloomers.[webapplication].[DeliveryLists] A (NOLOCK)
                        WHERE
                        AND A.NB_DOC_REMETENTE = '{cnpj_emp}' 
                        AND A.colletedAt IS NOT NULL
                        AND A.carrier = '{cod_transportadora}'
                        AND A.printedAt >= CONVERT(DATE, '{data_inicial.Trim()}')
                        AND A.printedAt <= CONCAT (CONVERT(DATE, '{data_final.Trim()}'),' 23:59:59')"")";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.QueryAsync<DeliveryList>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [DeliveryList] - GetOrderShipped - Erro ao obter pedido: {nr_pedido} na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<bool?> InsertPickedsDates(Guid guid, string deliveryListName, string carrier, IEnumerable<Order> orders)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var sql = @$"INSERT INTO azure.newbloomers.[webapplication].[DeliveryLists]
                            ([uniqueidentifier], [name], [carrier], [printedAt]) 
                            Values 
                            (@uniqueidentifier, @name, @carrier, GETDATE())";

                    await conn.ExecuteAsync(sql: sql, new { uniqueidentifier = $"{guid}", name = $"{deliveryListName}", carrier = $"{carrier}", printedAt = DateTime.Now }, commandTimeout: 360);

                    foreach (var order in orders)
                    {
                        var _sql = @$"INSERT INTO azure.newbloomers.[webapplication].[DeliveryListsOrders]
                            ([uniqueidentifier], [pedido]) 
                            Values 
                            (@uniqueidentifier, @pedido)";

                        await conn.ExecuteAsync(sql: _sql, new { uniqueidentifier = $"{guid}", pedido = $"{order.number}" }, commandTimeout: 360);
                    }

                    //var result = await conn.ExecuteAsync($"exec azure.newbloomers.[general].[p_DeliveryLists_Sincronizacao]");

                    //var result = await conn.ExecuteAsync($"exec azure.newbloomers.[general].[p_DeliveryListsOrders_Sincronizacao]");
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

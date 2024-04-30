using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;

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
                        TRIM(A.DOCUMENTO) as number,
                        A.VOLUMES as volumes,
                                                            
                        A.NB_CODIGO_CLIENTE as cod_client,
                        A.NB_RAZAO_CLIENTE as reason_client,
                        A.NB_DOC_CLIENTE as doc_client,
                        A.NB_ENDERECO_CLIENTE as address_client,
                        A.NB_NUMERO_RUA_CLIENTE as street_number_client,
                        A.NB_COMPLEMENTO_END_CLIENTE as complement_address_client,
                        A.NB_BAIRRO_CLIENTE as neighborhood_client,
                        A.NB_CIDADE as city_client,
                        A.NB_ESTADO as uf_client,
                        TRIM(REPLACE(A.NB_CEP, '-', '')) as zip_code_client,
                        A.NB_FONE_CLIENTE as fone_client,
                        
                        A.NB_TRANSPORTADORA as cod_shippingCompany,
                        A.NB_RAZAO_TRANSPORTADORA as reason_shippingCompany,

                        A.NF_SAIDA as number_nf,
                        A.NB_VALOR_PEDIDO as amount_nf,

                        A.NB_DOC_REMETENTE as doc_company

                        FROM GENERAL..IT4_WMS_DOCUMENTO A
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
                var result = await  _conn.GetDbConnection().QueryAsync<Order, Client, ShippingCompany, Invoice, Company, Order>(sql, (pedido, cliente, transportadora, notaFiscal, empresa) =>
                {
                    pedido.client = cliente;
                    pedido.shippingCompany = transportadora;
                    pedido.invoice = notaFiscal;
                    pedido.company = empresa;
                    return pedido;
                }, splitOn: "cod_client, cod_shippingCompany, number_nf, doc_company");

                return result.First();
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [DeliveryList] - GetOrderShipped - Erro ao obter pedido: {nr_pedido} na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<IEnumerable<Order>?> GetOrdersShipped(string cod_transportadora, string cnpj_emp, string serie_pedido, string data_inicial, string data_final)
        {
            var sql = String.Empty;

            if (cod_transportadora == "7601")
            {
                sql = $@"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) as number,
                        A.VOLUMES as volumes,
                                                            
                        A.NB_CODIGO_CLIENTE as cod_client,
                        A.NB_RAZAO_CLIENTE as reason_client,
                        A.NB_DOC_CLIENTE as doc_client,
                        A.NB_ENDERECO_CLIENTE as address_client,
                        A.NB_NUMERO_RUA_CLIENTE as street_number_client,
                        A.NB_COMPLEMENTO_END_CLIENTE as complement_address_client,
                        A.NB_BAIRRO_CLIENTE as neighborhood_client,
                        A.NB_CIDADE as city_client,
                        A.NB_ESTADO as uf_client,
                        TRIM(REPLACE(A.NB_CEP, '-', '')) as zip_code_client,
                        A.NB_FONE_CLIENTE as fone_client,
                        
                        A.NB_TRANSPORTADORA as cod_shippingCompany,
                        A.NB_RAZAO_TRANSPORTADORA as reason_shippingCompany,

                        A.NF_SAIDA as number_nf,
                        A.NB_VALOR_PEDIDO as amount_nf,

                        A.NB_DOC_REMETENTE as doc_company

                        FROM GENERAL..IT4_WMS_DOCUMENTO A
                        JOIN GENERAL..TOTALEXPRESSREGISTROLOG B ON A.DOCUMENTO = B.PEDIDO
                        WHERE
                        A.SERIE = '{serie_pedido}'
                        AND A.NB_DOC_REMETENTE = '{cnpj_emp}'
                        AND A.CHAVE_NFE IS NOT NULL
                        AND A.XML_FATURAMENTO IS NOT NULL
                        AND A.NF_SAIDA IS NOT NULL
                        AND A.NB_TRANSPORTADORA = '{cod_transportadora}'
                        AND B.DATAENVIO >= CONVERT(DATE, '{data_inicial.Trim()}')
                        AND B.DATAENVIO <= CONCAT (CONVERT(DATE, '{data_final.Trim()}'),' 23:59:59')";
            }
            else
            {
                sql = $@"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) as number,
                        A.VOLUMES as volumes,
                                                            
                        A.NB_CODIGO_CLIENTE as cod_client,
                        A.NB_RAZAO_CLIENTE as reason_client,
                        A.NB_DOC_CLIENTE as doc_client,
                        A.NB_ENDERECO_CLIENTE as address_client,
                        A.NB_NUMERO_RUA_CLIENTE as street_number_client,
                        A.NB_COMPLEMENTO_END_CLIENTE as complement_address_client,
                        A.NB_BAIRRO_CLIENTE as neighborhood_client,
                        A.NB_CIDADE as city_client,
                        A.NB_ESTADO as uf_client,
                        TRIM(REPLACE(A.NB_CEP, '-', '')) as zip_code_client,
                        A.NB_FONE_CLIENTE as fone_client,
                        
                        A.NB_TRANSPORTADORA as cod_shippingCompany,
                        A.NB_RAZAO_TRANSPORTADORA as reason_shippingCompany,

                        A.NF_SAIDA as number_nf,
                        A.NB_VALOR_PEDIDO as amount_nf,

                        A.NB_DOC_REMETENTE as doc_company

                        FROM GENERAL..IT4_WMS_DOCUMENTO A
                        WHERE
                        A.SERIE = '{serie_pedido}'
                        AND A.NB_DOC_REMETENTE = '{cnpj_emp}'
                        AND A.CHAVE_NFE IS NOT NULL
                        AND A.XML_FATURAMENTO IS NOT NULL
                        AND A.NF_SAIDA IS NOT NULL
                        AND A.NB_TRANSPORTADORA = '{cod_transportadora}'
                        AND A.RETORNO > '{data_inicial}'
                        AND A.RETORNO < '{data_final}'";
            }

            try
            {
                return await _conn.GetDbConnection().QueryAsync<Order, Client, ShippingCompany, Invoice, Company, Order>(sql, (pedido, cliente, transportadora, notaFiscal, empresa) =>
                {
                    pedido.client = cliente;
                    pedido.shippingCompany = transportadora;
                    pedido.invoice = notaFiscal;
                    pedido.company = empresa;
                    return pedido;
                }, splitOn: "cod_client, cod_shippingCompany, number_nf, doc_company");
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Imprime Romaneio] - GetPedidosEnviados - Erro ao obter pedidos na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }
    }
}

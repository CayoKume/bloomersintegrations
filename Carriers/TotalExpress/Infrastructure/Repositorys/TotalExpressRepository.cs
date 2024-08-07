using BloomersCarriersIntegrations.TotalExpress.Domain.Entities;
using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;
using Order = BloomersCarriersIntegrations.TotalExpress.Domain.Entities.Order;

namespace BloomersCarriersIntegrations.TotalExpress.Infrastructure.Repositorys
{
    public class TotalExpressRepository : ITotalExpressRepository
    {
        private readonly ISQLServerConnection _conn;

        public TotalExpressRepository(ISQLServerConnection conn) =>
            _conn = conn;

        public async Task GeraResponseLog(string pedido, string remetenteId, string response)
        {
            var sql = @$"INSERT INTO [GENERAL].[dbo].TotalExpressRegistroLog (pedido, remetenteid, dataenvio, retorno) 
                         VALUES ('{pedido}', '{remetenteId}', GETDATE(), '{response}')";
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - GenerateLog - Erro ao inserir registro: {pedido} na tabela GENERAL..TotalExpressRegistroLog - {ex.Message}");
            }
        }

        public async Task GeraRequestLog(string pedido, string request)
        {
            var sql = @$"INSERT INTO [GENERAL].[dbo].TotalExpressRequestLog (pedido, dataenvio, request) 
                         VALUES ('{pedido}', GETDATE(), '{request.Replace("'", "''")}')";
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - GenerateRequestLog - Erro ao inserir registro: {pedido} na tabela GENERAL..TotalExpressRequestLog - {ex.Message}");
            }
        }

        public async Task<Order> GetInvoicedOrder(string nr_pedido)
        {
            var sql = $@"SELECT DISTINCT 
                                 TRIM(A.DOCUMENTO) AS NUMBER,
                                 A.VOLUMES AS VOLUMES,
                                 A.NB_CFOP_PEDIDO AS CFOP,

                                 (SELECT REMETENTE_ID FROM GENERAL..PARAMETROS_TOTALEXPRESS WITH (NOLOCK) WHERE PRODUTO = 
                                     CASE 
                                         WHEN A.DOCUMENTO LIKE '%OA-LJ%' THEN 'OPEN ERA - B2B'
                                         WHEN A.DOCUMENTO LIKE '%MI-LJ%' THEN 'MISHA - B2B'
                                 
            	                         WHEN A.NB_DOC_REMETENTE = '38367316000431' THEN 'MISHA - SHIP FROM STORE - IGUATEMI'
            	                         WHEN A.NB_DOC_REMETENTE = '38367316000270' THEN 'MISHA - SHIP FROM STORE - JK IGUATEMI'
            	                         WHEN A.NB_DOC_REMETENTE = '38367316000350' THEN 'MISHA - SHIP FROM STORE - MORUMBI'
            	                         WHEN A.NB_DOC_REMETENTE = '38367316000512' THEN 'MISHA - SHIP FROM STORE - BARRA DA TIJUCA'
            	                         WHEN A.NB_DOC_REMETENTE = '38367316000601' THEN 'MISHA - SHIP FROM STORE - LEBLON'
            	                         WHEN A.NB_DOC_REMETENTE = '38367316000784' THEN 'MISHA - SHIP FROM STORE - HIGIENOPOLIS'
            	                         WHEN A.NB_DOC_REMETENTE = '38367316000946' THEN 'MISHA - SHIP FROM STORE - BATEL'
            	                         WHEN A.NB_DOC_REMETENTE = '38367316001080' THEN 'MISHA - SHIP FROM STORE - SHOP BH'
            	                         WHEN A.NB_DOC_REMETENTE = '42538267000349' THEN 'OPEN ERA - SHIP FROM STORE - JK IGUATEMI'
            	                         WHEN A.NB_DOC_REMETENTE = '42538267000420' THEN 'OPEN ERA - SHIP FROM STORE - CAMPINAS'
            	                         WHEN A.NB_DOC_REMETENTE = '42538267000500' THEN 'OPEN ERA - SHIP FROM STORE - BATEL'
                                         WHEN A.NB_DOC_REMETENTE = '38367316001160' THEN 'MISHA - SHIP FROM STORE - ANALIA FRANCO'
                                 
                                         WHEN A.NB_METODO_TRANSPORTADORA = 'ESTD' AND A.SERIE = 'MI-' THEN 'MISHA - STANDARD'
                                         WHEN A.NB_METODO_TRANSPORTADORA = 'ETUR' AND A.SERIE = 'MI-' THEN 'MISHA - EXPRESSO'
                                         WHEN A.NB_METODO_TRANSPORTADORA = 'ESTD' AND A.SERIE = 'OA-' THEN 'OPEN ERA - STANDARD'
                                         WHEN A.NB_METODO_TRANSPORTADORA = 'ETUR' AND A.SERIE = 'OA-' THEN 'OPEN ERA - EXPRESSO'
                                     END
                                 ) AS REMETENTEID,
                                 
                                 (SELECT SERVICO_TIPO FROM GENERAL..PARAMETROS_TOTALEXPRESS WITH (NOLOCK) WHERE PRODUTO =
                                    CASE 
                                        WHEN A.DOCUMENTO LIKE '%OA-LJ%' THEN 'OPEN ERA - B2B'
                                        WHEN A.DOCUMENTO LIKE '%MI-LJ%' THEN 'MISHA - B2B'
                                    
                                        WHEN A.NB_DOC_REMETENTE = '38367316000431' THEN 'MISHA - SHIP FROM STORE - IGUATEMI'
            	                        WHEN A.NB_DOC_REMETENTE = '38367316000270' THEN 'MISHA - SHIP FROM STORE - JK IGUATEMI'
            	                        WHEN A.NB_DOC_REMETENTE = '38367316000350' THEN 'MISHA - SHIP FROM STORE - MORUMBI'
            	                        WHEN A.NB_DOC_REMETENTE = '38367316000512' THEN 'MISHA - SHIP FROM STORE - BARRA DA TIJUCA'
            	                        WHEN A.NB_DOC_REMETENTE = '38367316000601' THEN 'MISHA - SHIP FROM STORE - LEBLON'
            	                        WHEN A.NB_DOC_REMETENTE = '38367316000784' THEN 'MISHA - SHIP FROM STORE - HIGIENOPOLIS'
            	                        WHEN A.NB_DOC_REMETENTE = '38367316000946' THEN 'MISHA - SHIP FROM STORE - BATEL'
            	                        WHEN A.NB_DOC_REMETENTE = '38367316001080' THEN 'MISHA - SHIP FROM STORE - SHOP BH'
            	                        WHEN A.NB_DOC_REMETENTE = '42538267000349' THEN 'OPEN ERA - SHIP FROM STORE - JK IGUATEMI'
            	                        WHEN A.NB_DOC_REMETENTE = '42538267000420' THEN 'OPEN ERA - SHIP FROM STORE - CAMPINAS'
            	                        WHEN A.NB_DOC_REMETENTE = '42538267000500' THEN 'OPEN ERA - SHIP FROM STORE - BATEL'
                                        WHEN A.NB_DOC_REMETENTE = '38367316001160' THEN 'MISHA - SHIP FROM STORE - ANALIA FRANCO'
                                        
                                        WHEN A.NB_METODO_TRANSPORTADORA = 'ESTD' AND A.SERIE = 'MI-' THEN 'MISHA - STANDARD'
                                        WHEN A.NB_METODO_TRANSPORTADORA = 'ETUR' AND A.SERIE = 'MI-' THEN 'MISHA - EXPRESSO'
                                        WHEN A.NB_METODO_TRANSPORTADORA = 'ESTD' AND A.SERIE = 'OA-' THEN 'OPEN ERA - STANDARD'
                                        WHEN A.NB_METODO_TRANSPORTADORA = 'ETUR' AND A.SERIE = 'OA-' THEN 'OPEN ERA - EXPRESSO'
                                    END
                                 ) AS SERVICOTIPO,

                                 B.CODIGO_BARRA AS COD_PRODUCT,
                                 B.NB_SKU_PRODUTO AS SKU_PRODUCT,
                                 B.DESCRICAO AS DESCRIPTION_PRODUCT,
                                 B.CODIGO_BARRA AS COD_EAN_PRODUCT,
                                 B.QTDE AS QUANTITY_PRODUCT,
                                 B.NB_VALOR_UNITARIO_PRODUTO AS UNITARY_VALUE_PRODUCT,
                                 B.NB_VALOR_TOTAL_PRODUTO AS AMOUNT_PRODUCT,
                                 B.NB_VALOR_FRETE_PRODUTO AS SHIPPING_VALUE_PRODUCT,

                                 A.NB_CODIGO_CLIENTE AS COD_CLIENT,
                                 A.NB_RAZAO_CLIENTE AS REASON_CLIENT,
                                 A.NB_NOME_CLIENTE AS NAME_CLIENT,
                                 A.NB_DOC_CLIENTE AS DOC_CLIENT,
                                 A.NB_EMAIL_CLIENTE AS EMAIL_CLIENT,
                                 A.NB_ENDERECO_CLIENTE AS ADDRESS_CLIENT,
                                 CASE
                                     WHEN A.NB_NUMERO_RUA_CLIENTE = '' THEN 'SVN'
                                     ELSE A.NB_NUMERO_RUA_CLIENTE
                                 END AS STREET_NUMBER_CLIENT,
                                 A.NB_COMPLEMENTO_END_CLIENTE AS COMPLEMENT_ADDRESS_CLIENT,
                                 A.NB_BAIRRO_CLIENTE AS NEIGHBORHOOD_CLIENT,
                                 A.NB_CIDADE AS CITY_CLIENT,
                                 A.NB_ESTADO AS UF_CLIENT,
                                 A.NB_CEP AS ZIP_CODE_CLIENT,
                                 A.NB_FONE_CLIENTE AS FONE_CLIENT,
                                 A.NB_INSCRICAO_ESTADUAL_CLIENTE AS STATE_REGISTRATION_CLIENT,
                                 A.NB_INSCRICAO_MUNICIPAL_CLIENTE AS MUNICIPAL_REGISTRATION_CLIENT,

                                 A.NB_TRANSPORTADORA AS COD_SHIPPINGCOMPANY,
                                 A.NB_METODO_TRANSPORTADORA AS METODO_SHIPPINGCOMPANY,
                                 A.NB_RAZAO_TRANSPORTADORA AS REASON_SHIPPINGCOMPANY,
                                 A.NB_NOME_TRANSPORTADORA AS NAME_SHIPPINGCOMPANY,
                                 A.NB_DOC_TRANSPORTADORA AS DOC_SHIPPINGCOMPANY,
                                 A.NB_EMAIL_TRANSPORTADORA AS EMAIL_SHIPPINGCOMPANY,
                                 A.NB_ENDERECO_TRANSPORTADORA AS ADDRESS_SHIPPINGCOMPANY,
                                 A.NB_NUMERO_RUA_TRANSPORTADORA AS STREET_NUMBER_SHIPPINGCOMPANY,
                                 A.NB_COMPLEMENTO_END_TRANSPORTADORA AS COMPLEMENT_ADDRESS_SHIPPINGCOMPANY,
                                 A.NB_BAIRRO_TRANSPORTADORA AS NEIGHBORHOOD_SHIPPINGCOMPANY,
                                 A.NB_CIDADE_TRANSPORTADORA AS CITY_SHIPPINGCOMPANY,
                                 A.NB_UF_TRANSPORTADORA AS UF_SHIPPINGCOMPANY,
                                 A.NB_CEP_TRANSPORTADORA AS ZIP_CODE_SHIPPINGCOMPANY,
                                 A.NB_FONE_TRANSPORTADORA AS FONE_SHIPPINGCOMPANY,
                                 A.NB_INSCRICAO_ESTADUAL_TRANSPORTADORA AS STATE_REGISTRATION_SHIPPINGCOMPANY,

                                 A.NB_COD_REMETENTE AS COD_COMPANY,
                                 A.NB_RAZAO_REMETENTE AS REASON_COMPANY,
                                 A.NB_NOME_REMETENTE AS NAME_COMPANY,

                                 CASE
                                     WHEN A.NB_DOC_REMETENTE = '38367316000865' THEN '38367316000199' --ENVIA PEDIDOS PARA TOTAL DA MISHA - VOLO COMO MISHA - MATRIZ
                                     ELSE A.NB_DOC_REMETENTE
                                 END AS DOC_COMPANY,

                                 A.NB_EMAIL_REMETENTE AS EMAIL_COMPANY,
                                 A.NB_ENDERECO_REMETENTE AS ADDRESS_COMPANY,
                                 A.NB_NUMERO_RUA_REMETENTE AS STREET_NUMBER_COMPANY,
                                 A.NB_COMPLEMENTO_END_REMETENTE AS COMPLEMENT_ADDRESS_COMPANY,
                                 A.NB_BAIRRO_REMETENTE AS NEIGHBORHOOD_COMPANY,
                                 A.NB_CIDADE_REMETENTE AS CITY_COMPANY,
                                 A.NB_UF_REMETENTE AS UF_COMPANY,
                                 A.NB_CEP_REMETENTE AS ZIP_CODE_COMPANY,
                                 A.NB_FONE_REMETENTE AS FONE_COMPANY,
                                 A.NB_INSCRICAO_ESTADUAL_REMETENTE AS STATE_REGISTRATION_COMPANY,

                                 A.NF_SAIDA AS NUMBER_NF,
                                 A.NB_VALOR_PEDIDO AS AMOUNT_NF,
                                 (SELECT CAST(SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<vFrete>', CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 8, 4) AS DECIMAL(14,2))) AS SHIPPING_VALUE_NF,
                                 (SELECT CAST(SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<pesoB>', CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 7, 4) AS DECIMAL(14,2))) AS WEIGHT_NF,
                                 A.CHAVE_NFE AS KEY_NFE_NF,
                                 CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX)) AS XML_NF,
                                 CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX)) AS XML_DISTRIBUITION_NF,
                                 'NF' as TYPE_NF,
                                 (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<serie>', A.[XML_FATURAMENTO]) + 7, 1)) AS SERIE_NF,
                                 (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhEmi>', A.[XML_FATURAMENTO]) + 7, 25)) AS DATE_EMISSION_NF

                                 FROM [GENERAL].[dbo].[IT4_WMS_DOCUMENTO] A (NOLOCK)
                                 JOIN [GENERAL].[dbo].[IT4_WMS_DOCUMENTO_ITEM] B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE
                                 LEFT JOIN GENERAL..TOTALEXPRESSREGISTROLOG C (NOLOCK) ON A.DOCUMENTO = C.PEDIDO

                                 WHERE
                                 A.DOCUMENTO IN ('{nr_pedido}')
                                 AND A.SERIE != 'MX-'
                                 AND A.ORIGEM = 'P'
                                 AND A.CHAVE_NFE IS NOT NULL 
                                 AND A.XML_FATURAMENTO IS NOT NULL 
                                 --AND (C.PEDIDO IS NULL OR SUBSTRING(C.retorno, 3, 7) <> 'retorno') 
                                 --AND NOT EXISTS (SELECT 0 FROM GENERAL..TotalExpressRegistroLog TER (NOLOCK) WHERE TER.pedido = C.pedido AND SUBSTRING(TER.retorno, 3, 7) = 'retorno')";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Product, Client, ShippingCompany, Company, Invoice, Order>(sql, (order, product, client, shippingCompany, company, invoice) =>
                    {
                        order.itens.Add(product);
                        order.client = client;
                        order.shippingCompany = shippingCompany;
                        order.company = company;
                        order.invoice = invoice;
                        return order;
                    }, splitOn: "COD_PRODUCT, COD_CLIENT, COD_SHIPPINGCOMPANY, COD_COMPANY, NUMBER_NF", commandTimeout: 360);

                    var order = result.GroupBy(p => p.number).Select(g =>
                    {
                        var groupedOrder = g.First();
                        groupedOrder.itens = g.Select(p => p.itens.Single()).ToList();
                        return groupedOrder;
                    });

                    return order.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - GetPedidoFaturado - Erro ao obter pedido: {nr_pedido} da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<List<Order>> GetInvoicedOrders()
        {
            var sql = $@"SELECT DISTINCT 
                            TRIM(A.DOCUMENTO) AS NUMBER,
                            A.VOLUMES AS VOLUMES,
                            A.NB_CFOP_PEDIDO AS CFOP,

                            (SELECT REMETENTE_ID FROM GENERAL..PARAMETROS_TOTALEXPRESS WITH (NOLOCK) WHERE PRODUTO = 
                                CASE 
                                    WHEN A.DOCUMENTO LIKE '%OA-LJ%' THEN 'OPEN ERA - B2B'
                                    WHEN A.DOCUMENTO LIKE '%MI-LJ%' THEN 'MISHA - B2B'
                                 
            	                    WHEN A.NB_DOC_REMETENTE = '38367316000431' THEN 'MISHA - SHIP FROM STORE - IGUATEMI'
            	                    WHEN A.NB_DOC_REMETENTE = '38367316000270' THEN 'MISHA - SHIP FROM STORE - JK IGUATEMI'
            	                    WHEN A.NB_DOC_REMETENTE = '38367316000350' THEN 'MISHA - SHIP FROM STORE - MORUMBI'
            	                    WHEN A.NB_DOC_REMETENTE = '38367316000512' THEN 'MISHA - SHIP FROM STORE - BARRA DA TIJUCA'
            	                    WHEN A.NB_DOC_REMETENTE = '38367316000601' THEN 'MISHA - SHIP FROM STORE - LEBLON'
            	                    WHEN A.NB_DOC_REMETENTE = '38367316000784' THEN 'MISHA - SHIP FROM STORE - HIGIENOPOLIS'
            	                    WHEN A.NB_DOC_REMETENTE = '38367316000946' THEN 'MISHA - SHIP FROM STORE - BATEL'
            	                    WHEN A.NB_DOC_REMETENTE = '38367316001080' THEN 'MISHA - SHIP FROM STORE - SHOP BH'
            	                    WHEN A.NB_DOC_REMETENTE = '42538267000349' THEN 'OPEN ERA - SHIP FROM STORE - JK IGUATEMI'
            	                    WHEN A.NB_DOC_REMETENTE = '42538267000420' THEN 'OPEN ERA - SHIP FROM STORE - CAMPINAS'
            	                    WHEN A.NB_DOC_REMETENTE = '42538267000500' THEN 'OPEN ERA - SHIP FROM STORE - BATEL'
                                    WHEN A.NB_DOC_REMETENTE = '38367316001160' THEN 'MISHA - SHIP FROM STORE - ANALIA FRANCO'
                                 
                                    WHEN A.NB_METODO_TRANSPORTADORA = 'ESTD' AND A.SERIE = 'MI-' THEN 'MISHA - STANDARD'
                                    WHEN A.NB_METODO_TRANSPORTADORA = 'ETUR' AND A.SERIE = 'MI-' THEN 'MISHA - EXPRESSO'
                                    WHEN A.NB_METODO_TRANSPORTADORA = 'ESTD' AND A.SERIE = 'OA-' THEN 'OPEN ERA - STANDARD'
                                    WHEN A.NB_METODO_TRANSPORTADORA = 'ETUR' AND A.SERIE = 'OA-' THEN 'OPEN ERA - EXPRESSO'
                                END
                            ) AS REMETENTEID,
                                 
                            (SELECT SERVICO_TIPO FROM GENERAL..PARAMETROS_TOTALEXPRESS WITH (NOLOCK) WHERE PRODUTO =
                            CASE 
                                WHEN A.DOCUMENTO LIKE '%OA-LJ%' THEN 'OPEN ERA - B2B'
                                WHEN A.DOCUMENTO LIKE '%MI-LJ%' THEN 'MISHA - B2B'
                                    
                                WHEN A.NB_DOC_REMETENTE = '38367316000431' THEN 'MISHA - SHIP FROM STORE - IGUATEMI'
            	                WHEN A.NB_DOC_REMETENTE = '38367316000270' THEN 'MISHA - SHIP FROM STORE - JK IGUATEMI'
            	                WHEN A.NB_DOC_REMETENTE = '38367316000350' THEN 'MISHA - SHIP FROM STORE - MORUMBI'
            	                WHEN A.NB_DOC_REMETENTE = '38367316000512' THEN 'MISHA - SHIP FROM STORE - BARRA DA TIJUCA'
            	                WHEN A.NB_DOC_REMETENTE = '38367316000601' THEN 'MISHA - SHIP FROM STORE - LEBLON'
            	                WHEN A.NB_DOC_REMETENTE = '38367316000784' THEN 'MISHA - SHIP FROM STORE - HIGIENOPOLIS'
            	                WHEN A.NB_DOC_REMETENTE = '38367316000946' THEN 'MISHA - SHIP FROM STORE - BATEL'
            	                WHEN A.NB_DOC_REMETENTE = '38367316001080' THEN 'MISHA - SHIP FROM STORE - SHOP BH'
            	                WHEN A.NB_DOC_REMETENTE = '42538267000349' THEN 'OPEN ERA - SHIP FROM STORE - JK IGUATEMI'
            	                WHEN A.NB_DOC_REMETENTE = '42538267000420' THEN 'OPEN ERA - SHIP FROM STORE - CAMPINAS'
            	                WHEN A.NB_DOC_REMETENTE = '42538267000500' THEN 'OPEN ERA - SHIP FROM STORE - BATEL'
                                WHEN A.NB_DOC_REMETENTE = '38367316001160' THEN 'MISHA - SHIP FROM STORE - ANALIA FRANCO'
                                        
                                WHEN A.NB_METODO_TRANSPORTADORA = 'ESTD' AND A.SERIE = 'MI-' THEN 'MISHA - STANDARD'
                                WHEN A.NB_METODO_TRANSPORTADORA = 'ETUR' AND A.SERIE = 'MI-' THEN 'MISHA - EXPRESSO'
                                WHEN A.NB_METODO_TRANSPORTADORA = 'ESTD' AND A.SERIE = 'OA-' THEN 'OPEN ERA - STANDARD'
                                WHEN A.NB_METODO_TRANSPORTADORA = 'ETUR' AND A.SERIE = 'OA-' THEN 'OPEN ERA - EXPRESSO'
                            END
                            ) AS SERVICOTIPO,

                            B.CODIGO_BARRA AS COD_PRODUCT,
                            B.NB_SKU_PRODUTO AS SKU_PRODUCT,
                            B.DESCRICAO AS DESCRIPTION_PRODUCT,
                            B.CODIGO_BARRA AS COD_EAN_PRODUCT,
                            B.QTDE AS QUANTITY_PRODUCT,
                            B.NB_VALOR_UNITARIO_PRODUTO AS UNITARY_VALUE_PRODUCT,
                            B.NB_VALOR_TOTAL_PRODUTO AS AMOUNT_PRODUCT,
                            B.NB_VALOR_FRETE_PRODUTO AS SHIPPING_VALUE_PRODUCT,

                            A.NB_CODIGO_CLIENTE AS COD_CLIENT,
                            A.NB_RAZAO_CLIENTE AS REASON_CLIENT,
                            A.NB_NOME_CLIENTE AS NAME_CLIENT,
                            A.NB_DOC_CLIENTE AS DOC_CLIENT,
                            A.NB_EMAIL_CLIENTE AS EMAIL_CLIENT,
                            A.NB_ENDERECO_CLIENTE AS ADDRESS_CLIENT,
                            CASE
                                WHEN A.NB_NUMERO_RUA_CLIENTE = '' THEN 'SVN'
                                ELSE A.NB_NUMERO_RUA_CLIENTE
                            END AS STREET_NUMBER_CLIENT,
                            A.NB_COMPLEMENTO_END_CLIENTE AS COMPLEMENT_ADDRESS_CLIENT,
                            A.NB_BAIRRO_CLIENTE AS NEIGHBORHOOD_CLIENT,
                            A.NB_CIDADE AS CITY_CLIENT,
                            A.NB_ESTADO AS UF_CLIENT,
                            A.NB_CEP AS ZIP_CODE_CLIENT,
                            A.NB_FONE_CLIENTE AS FONE_CLIENT,
                            A.NB_INSCRICAO_ESTADUAL_CLIENTE AS STATE_REGISTRATION_CLIENT,
                            A.NB_INSCRICAO_MUNICIPAL_CLIENTE AS MUNICIPAL_REGISTRATION_CLIENT,

                            A.NB_TRANSPORTADORA AS COD_SHIPPINGCOMPANY,
                            A.NB_METODO_TRANSPORTADORA AS METODO_SHIPPINGCOMPANY,
                            A.NB_RAZAO_TRANSPORTADORA AS REASON_SHIPPINGCOMPANY,
                            A.NB_NOME_TRANSPORTADORA AS NAME_SHIPPINGCOMPANY,
                            A.NB_DOC_TRANSPORTADORA AS DOC_SHIPPINGCOMPANY,
                            A.NB_EMAIL_TRANSPORTADORA AS EMAIL_SHIPPINGCOMPANY,
                            A.NB_ENDERECO_TRANSPORTADORA AS ADDRESS_SHIPPINGCOMPANY,
                            A.NB_NUMERO_RUA_TRANSPORTADORA AS STREET_NUMBER_SHIPPINGCOMPANY,
                            A.NB_COMPLEMENTO_END_TRANSPORTADORA AS COMPLEMENT_ADDRESS_SHIPPINGCOMPANY,
                            A.NB_BAIRRO_TRANSPORTADORA AS NEIGHBORHOOD_SHIPPINGCOMPANY,
                            A.NB_CIDADE_TRANSPORTADORA AS CITY_SHIPPINGCOMPANY,
                            A.NB_UF_TRANSPORTADORA AS UF_SHIPPINGCOMPANY,
                            A.NB_CEP_TRANSPORTADORA AS ZIP_CODE_SHIPPINGCOMPANY,
                            A.NB_FONE_TRANSPORTADORA AS FONE_SHIPPINGCOMPANY,
                            A.NB_INSCRICAO_ESTADUAL_TRANSPORTADORA AS STATE_REGISTRATION_SHIPPINGCOMPANY,

                            A.NB_COD_REMETENTE AS COD_COMPANY,
                            A.NB_RAZAO_REMETENTE AS REASON_COMPANY,
                            A.NB_NOME_REMETENTE AS NAME_COMPANY,

                            CASE
                                WHEN A.NB_DOC_REMETENTE = '38367316000865' THEN '38367316000199' --ENVIA PEDIDOS PARA TOTAL DA MISHA - VOLO COMO MISHA - MATRIZ
                                ELSE A.NB_DOC_REMETENTE
                            END AS DOC_COMPANY,

                            A.NB_EMAIL_REMETENTE AS EMAIL_COMPANY,
                            A.NB_ENDERECO_REMETENTE AS ADDRESS_COMPANY,
                            A.NB_NUMERO_RUA_REMETENTE AS STREET_NUMBER_COMPANY,
                            A.NB_COMPLEMENTO_END_REMETENTE AS COMPLEMENT_ADDRESS_COMPANY,
                            A.NB_BAIRRO_REMETENTE AS NEIGHBORHOOD_COMPANY,
                            A.NB_CIDADE_REMETENTE AS CITY_COMPANY,
                            A.NB_UF_REMETENTE AS UF_COMPANY,
                            A.NB_CEP_REMETENTE AS ZIP_CODE_COMPANY,
                            A.NB_FONE_REMETENTE AS FONE_COMPANY,
                            A.NB_INSCRICAO_ESTADUAL_REMETENTE AS STATE_REGISTRATION_COMPANY,

                            A.NF_SAIDA AS NUMBER_NF,
                            A.NB_VALOR_PEDIDO AS AMOUNT_NF,
                            (SELECT CAST(SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<vFrete>', CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 8, 4) AS DECIMAL(14,2))) AS SHIPPING_VALUE_NF,
                            (SELECT CAST(SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<pesoB>', CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 7, 4) AS DECIMAL(14,2))) AS WEIGHT_NF,
                            A.CHAVE_NFE AS KEY_NFE_NF,
                            CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX)) AS XML_NF,
                            CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX)) AS XML_DISTRIBUITION_NF,
                            'NF' as TYPE_NF,
                            (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<serie>', A.[XML_FATURAMENTO]) + 7, 1)) AS SERIE_NF,
                            (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhEmi>', A.[XML_FATURAMENTO]) + 7, 25)) AS DATE_EMISSION_NF

                            FROM [GENERAL].[dbo].[IT4_WMS_DOCUMENTO] A (NOLOCK)
                            JOIN [GENERAL].[dbo].[IT4_WMS_DOCUMENTO_ITEM] B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE
                            LEFT JOIN GENERAL..TOTALEXPRESSREGISTROLOG C (NOLOCK) ON A.DOCUMENTO = C.PEDIDO
                            WHERE
                            --A.DOCUMENTO IN ('')
                            A.SERIE != 'MX-'
							AND A.ORIGEM = 'P'
                            AND A.CHAVE_NFE IS NOT NULL 
                            AND A.XML_FATURAMENTO IS NOT NULL 
                            AND A.NB_TRANSPORTADORA = '7601'
                            AND A.DATA > GETDATE() - 15
                            AND A.VOLUMES IS NOT NULL
							AND (C.PEDIDO IS NULL OR SUBSTRING(C.retorno, 3, 7) <> 'retorno') 
							AND NOT EXISTS (SELECT 0 FROM GENERAL..TotalExpressRegistroLog TER (NOLOCK) WHERE TER.pedido = C.pedido AND SUBSTRING(TER.retorno, 3, 7) = 'retorno')";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Product, Client, ShippingCompany, Company, Invoice, Order>(sql, (order, product, client, shippingCompany, company, invoice) =>
                    {
                        order.itens.Add(product);
                        order.client = client;
                        order.shippingCompany = shippingCompany;
                        order.company = company;
                        order.invoice = invoice;
                        return order;
                    }, splitOn: "COD_PRODUCT, COD_CLIENT, COD_SHIPPINGCOMPANY, COD_COMPANY, NUMBER_NF", commandTimeout: 360);

                    var orders = result.GroupBy(p => p.number).Select(g =>
                    {
                        var groupedOrder = g.First();
                        groupedOrder.itens = g.Select(p => p.itens.Single()).ToList();
                        return groupedOrder;
                    });

                    return orders.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - GetPedidosFaturados - Erro ao obter pedidos da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<IEnumerable<TotalExpressRegistroLog>> GetPedidosEnviados()
        {
            try
            {
                var sql = $@"SELECT DISTINCT B.PEDIDO, B.REMETENTEID, A.NB_PREVISAO_REAL_ENTREGA AS PREVISAO_REAL_ENTREGA, A.NB_DESCRICAO_ULTIMO_STATUS AS DESCRICAO_ULTIMO_STATUS, A.NB_DATA_ENTREGA_REALIZADA AS ENTREGA_REALIZADA, A.NB_DATA_COLETA AS DATA_COLETA
                            FROM [GENERAL].[dbo].[IT4_WMS_DOCUMENTO] A (NOLOCK)
                            JOIN [GENERAL].[dbo].[TOTALEXPRESSREGISTROLOG] B (NOLOCK) ON B.PEDIDO = A.DOCUMENTO
                            WHERE
                            --A.DOCUMENTO = ''
							B.PEDIDO IS NOT NULL
							AND A.NB_DATA_ENTREGA_REALIZADA IS NULL
                            
                            AND A.VOLUMES = 1 
                            AND A.CHAVE_NFE IS NOT NULL
                            AND A.NF_SAIDA IS NOT NULL
                            AND A.NB_TRANSPORTADORA = 7601 
                            AND A.DATA > GETDATE() - 15";

                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryAsync<TotalExpressRegistroLog>(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - GetPedidosEnviados - Erro ao obter pedidos enviados da tabela GENERAL..TotalExpressRegistroLog - {ex.Message}");
            }
        }

        public async Task Update_NB_DATA_COLETA(string data, string nr_pedido)
        {
            string sql = $"UPDATE GENERAL..[IT4_WMS_DOCUMENTO] SET NB_DATA_COLETA = convert(datetime, '{data}') WHERE LTRIM(RTRIM(Documento)) = '{nr_pedido}' AND NB_TRANSPORTADORA = '7601'; ";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - Update_NB_DATA_COLETA - Erro ao atuaizar NB_DATA_COLETA do pedido: {nr_pedido} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task Update_NB_PREVISAO_REAL_ENTREGA(string data, string nr_pedido)
        {
            string sql = $"UPDATE GENERAL..[IT4_WMS_DOCUMENTO] SET NB_PREVISAO_REAL_ENTREGA = convert(datetime, '{data}') WHERE LTRIM(RTRIM(Documento)) = '{nr_pedido}' AND NB_TRANSPORTADORA = '7601';";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - Update_NB_PREVISAO_REAL_ENTREGA - Erro ao atuaizar NB_PREVISAO_REAL_ENTREGA do pedido: {nr_pedido} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task Update_NB_DATA_ENTREGA_REALIZADA(string data, string nr_pedido)
        {
            string sql = @$"UPDATE GENERAL..[IT4_WMS_DOCUMENTO] SET NB_DATA_ENTREGA_REALIZADA = convert(datetime, '{data}') WHERE LTRIM(RTRIM(Documento)) = '{nr_pedido}' AND NB_TRANSPORTADORA = '7601';
                            UPDATE GENERAL..[TOTALEXPRESSREGISTROLOG] SET STATUSECOM = '56' WHERE PEDIDO = '{nr_pedido}'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - Update_NB_DATA_ENTREGA_REALIZADA - Erro ao atuaizar NB_DATA_ENTREGA_REALIZADA do pedido: {nr_pedido} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task Update_NB_DATA_ULTIMO_STATUS(string data, string eventoId, string evento, string nr_pedido)
        {
            string sql = $"UPDATE GENERAL..[IT4_WMS_DOCUMENTO] SET NB_DATA_ULTIMO_STATUS = convert(datetime, '{data}'), NB_DESCRICAO_ULTIMO_STATUS = '{eventoId}-{evento}' WHERE LTRIM(RTRIM(Documento)) = '{nr_pedido}' AND NB_TRANSPORTADORA = '7601'; ";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - Update_NB_DATA_ULTIMO_STATUS - Erro ao atuaizar NB_DATA_ULTIMO_STATUS do pedido: {nr_pedido} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<Order> GetInvoicedOrderETUR(string nr_pedido)
        {
            var sql = $@"SELECT DISTINCT 
                                 TRIM(A.DOCUMENTO) AS NUMBER,
                                 A.VOLUMES AS VOLUMES,
                                 A.NB_CFOP_PEDIDO AS CFOP,

                                 39833 AS REMETENTEID,
                                 7 AS SERVICOTIPO,

                                 B.CODIGO_BARRA AS COD_PRODUCT,
                                 B.NB_SKU_PRODUTO AS SKU_PRODUCT,
                                 B.DESCRICAO AS DESCRIPTION_PRODUCT,
                                 B.CODIGO_BARRA AS COD_EAN_PRODUCT,
                                 B.QTDE AS QUANTITY_PRODUCT,
                                 B.NB_VALOR_UNITARIO_PRODUTO AS UNITARY_VALUE_PRODUCT,
                                 B.NB_VALOR_TOTAL_PRODUTO AS AMOUNT_PRODUCT,
                                 B.NB_VALOR_FRETE_PRODUTO AS SHIPPING_VALUE_PRODUCT,

                                 A.NB_CODIGO_CLIENTE AS COD_CLIENT,
                                 A.NB_RAZAO_CLIENTE AS REASON_CLIENT,
                                 A.NB_NOME_CLIENTE AS NAME_CLIENT,
                                 A.NB_DOC_CLIENTE AS DOC_CLIENT,
                                 A.NB_EMAIL_CLIENTE AS EMAIL_CLIENT,
                                 A.NB_ENDERECO_CLIENTE AS ADDRESS_CLIENT,
                                 CASE
                                     WHEN A.NB_NUMERO_RUA_CLIENTE = '' THEN 'SVN'
                                     ELSE A.NB_NUMERO_RUA_CLIENTE
                                 END AS STREET_NUMBER_CLIENT,
                                 A.NB_COMPLEMENTO_END_CLIENTE AS COMPLEMENT_ADDRESS_CLIENT,
                                 A.NB_BAIRRO_CLIENTE AS NEIGHBORHOOD_CLIENT,
                                 A.NB_CIDADE AS CITY_CLIENT,
                                 A.NB_ESTADO AS UF_CLIENT,
                                 A.NB_CEP AS ZIP_CODE_CLIENT,
                                 A.NB_FONE_CLIENTE AS FONE_CLIENT,
                                 A.NB_INSCRICAO_ESTADUAL_CLIENTE AS STATE_REGISTRATION_CLIENT,
                                 A.NB_INSCRICAO_MUNICIPAL_CLIENTE AS MUNICIPAL_REGISTRATION_CLIENT,

                                 A.NB_TRANSPORTADORA AS COD_SHIPPINGCOMPANY,
                                 A.NB_METODO_TRANSPORTADORA AS METODO_SHIPPINGCOMPANY,
                                 A.NB_RAZAO_TRANSPORTADORA AS REASON_SHIPPINGCOMPANY,
                                 A.NB_NOME_TRANSPORTADORA AS NAME_SHIPPINGCOMPANY,
                                 A.NB_DOC_TRANSPORTADORA AS DOC_SHIPPINGCOMPANY,
                                 A.NB_EMAIL_TRANSPORTADORA AS EMAIL_SHIPPINGCOMPANY,
                                 A.NB_ENDERECO_TRANSPORTADORA AS ADDRESS_SHIPPINGCOMPANY
                                 A.NB_NUMERO_RUA_TRANSPORTADORA AS STREET_NUMBER_SHIPPINGCOMPANY,
                                 A.NB_COMPLEMENTO_END_TRANSPORTADORA AS COMPLEMENT_ADDRESS_SHIPPINGCOMPANY,
                                 A.NB_BAIRRO_TRANSPORTADORA AS NEIGHBORHOOD_SHIPPINGCOMPANY,
                                 A.NB_CIDADE_TRANSPORTADORA AS CITY_SHIPPINGCOMPANY,
                                 A.NB_UF_TRANSPORTADORA AS UF_SHIPPINGCOMPANY,
                                 A.NB_CEP_TRANSPORTADORA AS ZIP_CODE_SHIPPINGCOMPANY,
                                 A.NB_FONE_TRANSPORTADORA AS FONE_SHIPPINGCOMPANY,
                                 A.NB_INSCRICAO_ESTADUAL_TRANSPORTADORA AS STATE_REGISTRATION_SHIPPINGCOMPANY,

                                 A.NB_COD_REMETENTE AS COD_COMPANY,
                                 A.NB_RAZAO_REMETENTE AS REASON_COMPANY,
                                 A.NB_NOME_REMETENTE AS NAME_COMPANY,

                                 CASE
                                     WHEN A.NB_DOC_REMETENTE = '38367316000865' THEN '38367316000199' --ENVIA PEDIDOS PARA TOTAL DA MISHA - VOLO COMO MISHA - MATRIZ
                                     ELSE A.NB_DOC_REMETENTE
                                 END AS DOC_COMPANY,

                                 A.NB_EMAIL_REMETENTE AS EMAIL_COMPANY,
                                 A.NB_ENDERECO_REMETENTE AS ADDRESS_COMPANY,
                                 A.NB_NUMERO_RUA_REMETENTE AS STREET_NUMBER_COMPANY,
                                 A.NB_COMPLEMENTO_END_REMETENTE AS COMPLEMENT_ADDRESS_COMPANY,
                                 A.NB_BAIRRO_REMETENTE AS NEIGHBORHOOD_COMPANY,
                                 A.NB_CIDADE_REMETENTE AS CITY_COMPANY,
                                 A.NB_UF_REMETENTE AS UF_COMPANY,
                                 A.NB_CEP_REMETENTE AS ZIP_CODE_COMPANY,
                                 A.NB_FONE_REMETENTE AS FONE_COMPANY,
                                 A.NB_INSCRICAO_ESTADUAL_REMETENTE AS STATE_REGISTRATION_COMPANY,

                                 A.NF_SAIDA AS NUMBER_NF,
                                 A.NB_VALOR_PEDIDO AS AMOUNT_NF,
                                 (SELECT CAST(SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<vFrete>', CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 8, 4) AS DECIMAL(14,2))) AS SHIPPING_VALUE_NF,
                                 (SELECT CAST(SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<pesoB>', CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 7, 4) AS DECIMAL(14,2))) AS WEIGHT_NF,
                                 A.CHAVE_NFE AS KEY_NFE_NF,
                                 CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX)) AS XML_NF,
                                 CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX)) AS XML_DISTRIBUITION_NF,
                                 'NF' as TYPE_NF,
                                 (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<serie>', A.[XML_FATURAMENTO]) + 7, 1)) AS SERIE_NF,
                                 (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhEmi>', A.[XML_FATURAMENTO]) + 7, 25)) AS DATE_EMISSION_NF

                                 FROM [GENERAL].[dbo].[IT4_WMS_DOCUMENTO] A (NOLOCK)
                                 JOIN [GENERAL].[dbo].[IT4_WMS_DOCUMENTO_ITEM] B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE
                                 LEFT JOIN GENERAL..TOTALEXPRESSREGISTROLOG C (NOLOCK) ON A.DOCUMENTO = C.PEDIDO

                                 WHERE
                                 A.DOCUMENTO IN ('{nr_pedido}')
                                 AND A.SERIE != 'MX-'
                                 AND A.ORIGEM = 'P'
                                 AND A.CHAVE_NFE IS NOT NULL 
                                 AND A.XML_FATURAMENTO IS NOT NULL 
                                 AND A.DATA > GETDATE() - 15
                                 --AND (C.PEDIDO IS NULL OR SUBSTRING(C.retorno, 3, 7) <> 'retorno') 
                                 --AND NOT EXISTS (SELECT 0 FROM GENERAL..TotalExpressRegistroLog TER (NOLOCK) WHERE TER.pedido = C.pedido AND SUBSTRING(TER.retorno, 3, 7) = 'retorno')";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Product, Client, ShippingCompany, Company, Invoice, Order>(sql, (order, product, client, shippingCompany, company, invoice) =>
                    {
                        order.itens.Add(product);
                        order.client = client;
                        order.shippingCompany = shippingCompany;
                        order.company = company;
                        order.invoice = invoice;
                        return order;
                    }, splitOn: "cod_product, cod_client, cod_shippingCompany, cod_company, number_nf", commandTimeout: 360);

                    var orders = result.GroupBy(p => p.number).Select(g =>
                    {
                        var groupedOrder = g.First();
                        groupedOrder.itens = g.Select(p => p.itens.Single()).ToList();
                        return groupedOrder;
                    });

                    return orders.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - GetPedidoFaturadoETUR - Erro ao obter pedido: {nr_pedido} da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task UpdatePedidoETUR(string nr_pedido)
        {
            string sql = @$"update GENERAL..IT4_WMS_DOCUMENTO set
                                    NB_transportadora = '7601',
                                    NB_metodo_transportadora = 'ETUR',
                                    NB_razao_transportadora = 'TEX COURIER S.A',
                                    NB_nome_transportadora = 'TOTAL EXPRESS',
                                    NB_doc_transportadora = '73939449000193',
                                    NB_email_transportadora = 'cpfiscal@totalexpress.com.br',
                                    NB_endereco_transportadora = 'Avenida Piracema',
                                    NB_numero_rua_transportadora = '155',
                                    NB_complemento_end_transportadora = 'GALPAO1',
                                    NB_bairro_transportadora = 'Tamboré',
                                    NB_cidade_transportadora = 'Barueri',
                                    NB_cep_transportadora = '06460-030',
                                    NB_uf_transportadora = 'SP',
                                    NB_fone_transportadora = '1136275900',
                                    NB_inscricao_estadual_transportadora = '206.214.714.111'
                                    where documento = '{nr_pedido}'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - UpdatePedidoETUR - Erro ao atuaizar dados da transportadora do pedido: {nr_pedido} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }
    }
}

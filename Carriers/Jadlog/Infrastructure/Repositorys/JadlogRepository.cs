using BloomersIntegrationsCore.Domain.Entities;
using Order = BloomersCarriersIntegrations.Jadlog.Domain.Entities.Order;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;
using BloomersCarriersIntegrations.Jadlog.Domain.Entities;
using Product = BloomersCarriersIntegrations.Jadlog.Domain.Entities.Product;

namespace BloomersCarriersIntegrations.Jadlog.Infrastructure.Repositorys
{
    public class JadlogRepository : IJadlogRepository
    {
        private readonly ISQLServerConnection _conn;

        public JadlogRepository(ISQLServerConnection conn) =>
            _conn = conn;

        public async Task GenerateRequestLog(string pedido, string request)
        {
            var sql = @$"INSERT INTO [GENERAL].[dbo].JadlogRequestLog (pedido, dataenvio, request) 
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
                throw new Exception(@$"Jadlog - GenerateRequestLog - Erro ao inserir registro: {pedido} na tabela GENERAL..JadlogRequestLog - {ex.Message}");
            }
        }

        public async Task GenerateResponseLog(string pedido, string response)
        {
            var sql = @$"INSERT INTO [GENERAL].[dbo].JadlogRegistroLog (pedido, dataenvio, response) 
                         VALUES ('{pedido}', GETDATE(), '{response}')";
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Jadlog - GenerateResponseLog - Erro ao inserir registro: {pedido} na tabela GENERAL..JadlogRegistroLog - {ex.Message}");
            }
        }

        public async Task<Order> GetInvoicedOrder(string number)
        {
            var sql = $@"SELECT DISTINCT 
                            TRIM(A.DOCUMENTO) AS NUMBER,
                            A.VOLUMES AS VOLUMES,
                            A.NB_CFOP_PEDIDO AS CFOP,
                            CASE
                                WHEN A.CLIENTE_OU_FORNECEDOR IN (SELECT COD_CLIENTE FROM BLOOMERS_LINX..LINXCLIENTESFORNEC_TRUSTED (NOLOCK) WHERE (RAZAO_CLIENTE LIKE '%MNR%')  AND TIPO_CLIENTE = 'J') THEN 'B2B' --B2B MISHA
                                WHEN A.CLIENTE_OU_FORNECEDOR IN (SELECT COD_CLIENTE FROM BLOOMERS_LINX..LINXCLIENTESFORNEC_TRUSTED (NOLOCK) WHERE (RAZAO_CLIENTE LIKE '%NEWFIT%')  AND TIPO_CLIENTE = 'J') THEN 'B2B' --B2B OPEN ERA
                                WHEN A.NB_COD_REMETENTE IN (SELECT EMPRESA FROM BLOOMERS_LINX..LINXLOJAS_TRUSTED (NOLOCK) WHERE EMPRESA NOT IN (0, 1, 5, 6, 10)) THEN 'SHIP FROM STORE' --SHIP FROM STORE OPEN ERA E MISHA (EXCLUI MISHA ECOMMERCE, OPEN ECOMMERCE, MARGAUX ECOMMERCE E NEWBLOOMERS)
                                ELSE 'B2C'
                            END AS TIPO_SERVICO,

                            B.CODIGO_BARRA AS COD_PRODUCT,
                            B.NB_SKU_PRODUTO AS SKU_PRODUCT,
                            B.DESCRICAO AS DESCRIPTION_PRODUCT,
                            B.CODIGO_BARRA AS COD_EAN_PRODUCT,
                            B.QTDE AS QUANTITY_PRODUCT,
                            B.NB_VALOR_UNITARIO_PRODUTO AS UNITARY_VALUE_PRODUCT,
                            B.NB_VALOR_TOTAL_PRODUTO AS AMOUNT_PRODUCT,
                            B.NB_VALOR_FRETE_PRODUTO AS SHIPPING_VALUE_PRODUCT,
                            B.NB_PESO_BRUTO AS WEIGHT_PRODUCT,

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
                            TRIM(A.NB_CEP) AS ZIP_CODE_CLIENT,
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
                            A.NB_DOC_REMETENTE AS DOC_COMPANY,
                            A.NB_EMAIL_REMETENTE AS EMAIL_COMPANY,
                            A.NB_ENDERECO_REMETENTE AS ADDRESS_COMPANY,
                            A.NB_NUMERO_RUA_REMETENTE AS STREET_NUMBER_COMPANY,
                            A.NB_COMPLEMENTO_END_REMETENTE AS COMPLEMENT_ADDRESS_COMPANY,
                            A.NB_BAIRRO_REMETENTE AS NEIGHBORHOOD_COMPANY,
                            A.NB_CIDADE_REMETENTE AS CITY_COMPANY,
                            A.NB_UF_REMETENTE AS UF_COMPANY,
                            REPLACE(A.NB_CEP_REMETENTE, '-', '') AS ZIP_CODE_COMPANY,
                            A.NB_FONE_REMETENTE AS FONE_COMPANY,
                            A.NB_INSCRICAO_ESTADUAL_REMETENTE AS STATE_REGISTRATION_COMPANY,

                            D.EMPRESA AS COD_COMPANY,
                            D.RAZAO_EMP AS REASON_COMPANY,
                            D.NOME_EMP AS NAME_COMPANY,
                            D.CNPJ_EMP AS DOC_COMPANY,
                            D.EMAIL_EMP AS EMAIL_COMPANY,
                            D.ENDERECO_EMP AS ADDRESS_COMPANY,
                            D.NUM_EMP AS STREET_NUMBER_COMPANY,
                            D.COMPLEMENT_EMP AS COMPLEMENT_ADDRESS_COMPANY,
                            D.BAIRRO_EMP AS NEIGHBORHOOD_COMPANY,
                            D.CIDADE_EMP AS CITY_COMPANY,
                            D.ESTADO_EMP AS UF_COMPANY,
                            REPLACE(D.CEP_EMP, '-', '') AS ZIP_CODE_COMPANY,
                            D.FONE_EMP AS FONE_COMPANY,
                            D.INSCRICAO_EMP AS STATE_REGISTRATION_COMPANY,

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
                            LEFT JOIN GENERAL..JADLOGREGISTROLOG C (NOLOCK) ON A.DOCUMENTO = C.PEDIDO
                            JOIN BLOOMERS_LINX..LINXLOJAS_TRUSTED D (NOLOCK) ON D.CNPJ_EMP = IIF(A.SERIE = 'MI-', '38367316000199', '42538267000187')

                            WHERE
                            A.DOCUMENTO IN ('{number}')
                            AND A.SERIE != 'MX-'
                            AND A.ORIGEM = 'P'
                            AND A.CHAVE_NFE IS NOT NULL 
                            AND A.XML_FATURAMENTO IS NOT NULL
                            AND A.NB_TRANSPORTADORA = '101988'
                            AND A.VOLUMES IS NOT NULL";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Product, Client, ShippingCompany, Company, Company, Invoice, Order>(sql, (order, product, client, shippingCompany, company, tomador, invoice) =>
                    {
                        order.itens.Add(product);
                        order.client = client;
                        order.shippingCompany = shippingCompany;
                        order.company = company;
                        order.tomador = tomador;
                        order.invoice = invoice;
                        return order;
                    }, splitOn: "COD_PRODUCT, COD_CLIENT, COD_SHIPPINGCOMPANY, COD_COMPANY, COD_COMPANY, NUMBER_NF", commandTimeout: 360);

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
                throw new Exception(@$"Jadlog - GetInvoicedOrder - Erro ao obter pedido: {number} da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        //public async Task<Order> GetInvoicedOrderETUR(string orderNumber)
        //{
        //    var sql = $@"SELECT DISTINCT 
        //                         TRIM(A.DOCUMENTO) AS NUMBER,
        //                         A.VOLUMES AS VOLUMES,
        //                         A.NB_CFOP_PEDIDO AS CFOP,

        //                         39833 AS REMETENTEID,
        //                         7 AS SERVICOTIPO,

        //                         B.CODIGO_BARRA AS COD_PRODUCT,
        //                         B.NB_SKU_PRODUTO AS SKU_PRODUCT,
        //                         B.DESCRICAO AS DESCRIPTION_PRODUCT,
        //                         B.CODIGO_BARRA AS COD_EAN_PRODUCT,
        //                         B.QTDE AS QUANTITY_PRODUCT,
        //                         B.NB_VALOR_UNITARIO_PRODUTO AS UNITARY_VALUE_PRODUCT,
        //                         B.NB_VALOR_TOTAL_PRODUTO AS AMOUNT_PRODUCT,
        //                         B.NB_VALOR_FRETE_PRODUTO AS SHIPPING_VALUE_PRODUCT,

        //                         A.NB_CODIGO_CLIENTE AS COD_CLIENT,
        //                         A.NB_RAZAO_CLIENTE AS REASON_CLIENT,
        //                         A.NB_NOME_CLIENTE AS NAME_CLIENT,
        //                         A.NB_DOC_CLIENTE AS DOC_CLIENT,
        //                         A.NB_EMAIL_CLIENTE AS EMAIL_CLIENT,
        //                         A.NB_ENDERECO_CLIENTE AS ADDRESS_CLIENT,
        //                         CASE
        //                             WHEN A.NB_NUMERO_RUA_CLIENTE = '' THEN 'SVN'
        //                             ELSE A.NB_NUMERO_RUA_CLIENTE
        //                         END AS STREET_NUMBER_CLIENT,
        //                         A.NB_COMPLEMENTO_END_CLIENTE AS COMPLEMENT_ADDRESS_CLIENT,
        //                         A.NB_BAIRRO_CLIENTE AS NEIGHBORHOOD_CLIENT,
        //                         A.NB_CIDADE AS CITY_CLIENT,
        //                         A.NB_ESTADO AS UF_CLIENT,
        //                         A.NB_CEP AS ZIP_CODE_CLIENT,
        //                         A.NB_FONE_CLIENTE AS FONE_CLIENT,
        //                         A.NB_INSCRICAO_ESTADUAL_CLIENTE AS STATE_REGISTRATION_CLIENT,
        //                         A.NB_INSCRICAO_MUNICIPAL_CLIENTE AS MUNICIPAL_REGISTRATION_CLIENT,

        //                         A.NB_TRANSPORTADORA AS COD_SHIPPINGCOMPANY,
        //                         A.NB_METODO_TRANSPORTADORA AS METODO_SHIPPINGCOMPANY,
        //                         A.NB_RAZAO_TRANSPORTADORA AS REASON_SHIPPINGCOMPANY,
        //                         A.NB_NOME_TRANSPORTADORA AS NAME_SHIPPINGCOMPANY,
        //                         A.NB_DOC_TRANSPORTADORA AS DOC_SHIPPINGCOMPANY,
        //                         A.NB_EMAIL_TRANSPORTADORA AS EMAIL_SHIPPINGCOMPANY,
        //                         A.NB_ENDERECO_TRANSPORTADORA AS ADDRESS_SHIPPINGCOMPANY
        //                         A.NB_NUMERO_RUA_TRANSPORTADORA AS STREET_NUMBER_SHIPPINGCOMPANY,
        //                         A.NB_COMPLEMENTO_END_TRANSPORTADORA AS COMPLEMENT_ADDRESS_SHIPPINGCOMPANY,
        //                         A.NB_BAIRRO_TRANSPORTADORA AS NEIGHBORHOOD_SHIPPINGCOMPANY,
        //                         A.NB_CIDADE_TRANSPORTADORA AS CITY_SHIPPINGCOMPANY,
        //                         A.NB_UF_TRANSPORTADORA AS UF_SHIPPINGCOMPANY,
        //                         A.NB_CEP_TRANSPORTADORA AS ZIP_CODE_SHIPPINGCOMPANY,
        //                         A.NB_FONE_TRANSPORTADORA AS FONE_SHIPPINGCOMPANY,
        //                         A.NB_INSCRICAO_ESTADUAL_TRANSPORTADORA AS STATE_REGISTRATION_SHIPPINGCOMPANY,

        //                         A.NB_COD_REMETENTE AS COD_COMPANY,
        //                         A.NB_RAZAO_REMETENTE AS REASON_COMPANY,
        //                         A.NB_NOME_REMETENTE AS NAME_COMPANY,

        //                         CASE
        //                             WHEN A.NB_DOC_REMETENTE = '38367316000865' THEN '38367316000199' --ENVIA PEDIDOS PARA TOTAL DA MISHA - VOLO COMO MISHA - MATRIZ
        //                             ELSE A.NB_DOC_REMETENTE
        //                         END AS DOC_COMPANY,

        //                         A.NB_EMAIL_REMETENTE AS EMAIL_COMPANY,
        //                         A.NB_ENDERECO_REMETENTE AS ADDRESS_COMPANY,
        //                         A.NB_NUMERO_RUA_REMETENTE AS STREET_NUMBER_COMPANY,
        //                         A.NB_COMPLEMENTO_END_REMETENTE AS COMPLEMENT_ADDRESS_COMPANY,
        //                         A.NB_BAIRRO_REMETENTE AS NEIGHBORHOOD_COMPANY,
        //                         A.NB_CIDADE_REMETENTE AS CITY_COMPANY,
        //                         A.NB_UF_REMETENTE AS UF_COMPANY,
        //                         A.NB_CEP_REMETENTE AS ZIP_CODE_COMPANY,
        //                         A.NB_FONE_REMETENTE AS FONE_COMPANY,
        //                         A.NB_INSCRICAO_ESTADUAL_REMETENTE AS STATE_REGISTRATION_COMPANY,

        //                         A.NF_SAIDA AS NUMBER_NF,
        //                         A.NB_VALOR_PEDIDO AS AMOUNT_NF,
        //                         (SELECT CAST(SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<vFrete>', CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 8, 4) AS DECIMAL(14,2))) AS SHIPPING_VALUE_NF,
        //                         (SELECT CAST(SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<pesoB>', CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 7, 4) AS DECIMAL(14,2))) AS WEIGHT_NF,
        //                         A.CHAVE_NFE AS KEY_NFE_NF,
        //                         CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX)) AS XML_NF,
        //                         CAST(A.[XML_FATURAMENTO] AS VARCHAR(MAX)) AS XML_DISTRIBUITION_NF,
        //                         'NF' as TYPE_NF,
        //                         (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<serie>', A.[XML_FATURAMENTO]) + 7, 1)) AS SERIE_NF,
        //                         (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhEmi>', A.[XML_FATURAMENTO]) + 7, 25)) AS DATE_EMISSION_NF

        //                         FROM [GENERAL].[dbo].[IT4_WMS_DOCUMENTO] A (NOLOCK)
        //                         JOIN [GENERAL].[dbo].[IT4_WMS_DOCUMENTO_ITEM] B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE
        //                         LEFT JOIN GENERAL..JADLOGREGISTROLOG C (NOLOCK) ON A.DOCUMENTO = C.PEDIDO

        //                         WHERE
        //                         A.DOCUMENTO IN ('{orderNumber}')
        //                         AND A.SERIE != 'MX-'
        //                         AND A.ORIGEM = 'P'
        //                         AND A.CHAVE_NFE IS NOT NULL 
        //                         AND A.XML_FATURAMENTO IS NOT NULL 
        //                         AND A.DATA > GETDATE() - 15";

        //    try
        //    {
        //        using (var conn = _conn.GetDbConnection())
        //        {
        //            var result = await conn.QueryAsync<Order, Product, Client, ShippingCompany, Company, Invoice, Order>(sql, (order, product, client, shippingCompany, company, invoice) =>
        //            {
        //                order.itens.Add(product);
        //                order.client = client;
        //                order.shippingCompany = shippingCompany;
        //                order.company = company;
        //                order.invoice = invoice;
        //                return order;
        //            }, splitOn: "cod_product, cod_client, cod_shippingCompany, cod_company, number_nf", commandTimeout: 360);

        //            var orders = result.GroupBy(p => p.number).Select(g =>
        //            {
        //                var groupedOrder = g.First();
        //                groupedOrder.itens = g.Select(p => p.itens.Single()).ToList();
        //                return groupedOrder;
        //            });

        //            return orders.First();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(@$"Jadlog - GetInvoicedOrderETUR - Erro ao obter pedido: {orderNumber} da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
        //    }
        //}

        public async Task<List<Order>> GetInvoicedOrders()
        {
            var sql = $@"SELECT DISTINCT 
                            TRIM(A.DOCUMENTO) AS NUMBER,
                            A.VOLUMES AS VOLUMES,
                            A.NB_CFOP_PEDIDO AS CFOP,
                            CASE
                                WHEN A.CLIENTE_OU_FORNECEDOR IN (SELECT COD_CLIENTE FROM BLOOMERS_LINX..LINXCLIENTESFORNEC_TRUSTED (NOLOCK) WHERE (RAZAO_CLIENTE LIKE '%MNR%')  AND TIPO_CLIENTE = 'J') THEN 'B2B' --B2B MISHA
                                WHEN A.CLIENTE_OU_FORNECEDOR IN (SELECT COD_CLIENTE FROM BLOOMERS_LINX..LINXCLIENTESFORNEC_TRUSTED (NOLOCK) WHERE (RAZAO_CLIENTE LIKE '%NEWFIT%')  AND TIPO_CLIENTE = 'J') THEN 'B2B' --B2B OPEN ERA
                                WHEN A.NB_COD_REMETENTE IN (SELECT EMPRESA FROM BLOOMERS_LINX..LINXLOJAS_TRUSTED (NOLOCK) WHERE EMPRESA NOT IN (0, 1, 5, 6, 10)) THEN 'SHIP FROM STORE' --SHIP FROM STORE OPEN ERA E MISHA (EXCLUI MISHA ECOMMERCE, OPEN ECOMMERCE, MARGAUX ECOMMERCE E NEWBLOOMERS)
                                ELSE 'B2C'
                            END AS TIPO_SERVICO,

                            B.CODIGO_BARRA AS COD_PRODUCT,
                            B.NB_SKU_PRODUTO AS SKU_PRODUCT,
                            B.DESCRICAO AS DESCRIPTION_PRODUCT,
                            B.CODIGO_BARRA AS COD_EAN_PRODUCT,
                            B.QTDE AS QUANTITY_PRODUCT,
                            B.NB_VALOR_UNITARIO_PRODUTO AS UNITARY_VALUE_PRODUCT,
                            B.NB_VALOR_TOTAL_PRODUTO AS AMOUNT_PRODUCT,
                            B.NB_VALOR_FRETE_PRODUTO AS SHIPPING_VALUE_PRODUCT,
                            B.NB_PESO_BRUTO AS WEIGHT_PRODUCT,

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
                            TRIM(A.NB_CEP) AS ZIP_CODE_CLIENT,
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
                            A.NB_DOC_REMETENTE AS DOC_COMPANY,
                            A.NB_EMAIL_REMETENTE AS EMAIL_COMPANY,
                            A.NB_ENDERECO_REMETENTE AS ADDRESS_COMPANY,
                            A.NB_NUMERO_RUA_REMETENTE AS STREET_NUMBER_COMPANY,
                            A.NB_COMPLEMENTO_END_REMETENTE AS COMPLEMENT_ADDRESS_COMPANY,
                            A.NB_BAIRRO_REMETENTE AS NEIGHBORHOOD_COMPANY,
                            A.NB_CIDADE_REMETENTE AS CITY_COMPANY,
                            A.NB_UF_REMETENTE AS UF_COMPANY,
                            REPLACE(A.NB_CEP_REMETENTE, '-', '') AS ZIP_CODE_COMPANY,
                            A.NB_FONE_REMETENTE AS FONE_COMPANY,
                            A.NB_INSCRICAO_ESTADUAL_REMETENTE AS STATE_REGISTRATION_COMPANY,

                            D.EMPRESA AS COD_COMPANY,
                            D.RAZAO_EMP AS REASON_COMPANY,
                            D.NOME_EMP AS NAME_COMPANY,
                            D.CNPJ_EMP AS DOC_COMPANY,
                            D.EMAIL_EMP AS EMAIL_COMPANY,
                            D.ENDERECO_EMP AS ADDRESS_COMPANY,
                            D.NUM_EMP AS STREET_NUMBER_COMPANY,
                            D.COMPLEMENT_EMP AS COMPLEMENT_ADDRESS_COMPANY,
                            D.BAIRRO_EMP AS NEIGHBORHOOD_COMPANY,
                            D.CIDADE_EMP AS CITY_COMPANY,
                            D.ESTADO_EMP AS UF_COMPANY,
                            REPLACE(D.CEP_EMP, '-', '') AS ZIP_CODE_COMPANY,
                            D.FONE_EMP AS FONE_COMPANY,
                            D.INSCRICAO_EMP AS STATE_REGISTRATION_COMPANY,

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
                            LEFT JOIN GENERAL..JADLOGREGISTROLOG C (NOLOCK) ON A.DOCUMENTO = C.PEDIDO
                            JOIN BLOOMERS_LINX..LINXLOJAS_TRUSTED D (NOLOCK) ON D.CNPJ_EMP = IIF(A.SERIE = 'MI-', '38367316000199', '42538267000187')
                            WHERE
                            --A.DOCUMENTO IN ('')
                            A.SERIE != 'MX-'
							AND A.ORIGEM = 'P'
                            AND A.CHAVE_NFE IS NOT NULL 
                            AND A.XML_FATURAMENTO IS NOT NULL 
                            AND A.NB_TRANSPORTADORA = '101988'
                            --AND A.DATA > GETDATE() - 15
                            AND A.VOLUMES IS NOT NULL
							AND (C.PEDIDO IS NULL OR SUBSTRING(C.RESPONSE, 3, 7) <> 'erro') 
							AND NOT EXISTS (SELECT 0 FROM GENERAL..JADLOGREGISTROLOG TER (NOLOCK) WHERE TER.PEDIDO = C.PEDIDO AND SUBSTRING(TER.RESPONSE, 3, 7) = 'erro')";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Product, Client, ShippingCompany, Company, Company, Invoice, Order>(sql, (order, product, client, shippingCompany, company, tomador, invoice) =>
                    {
                        order.itens.Add(product);
                        order.client = client;
                        order.shippingCompany = shippingCompany;
                        order.company = company;
                        order.tomador = tomador;
                        order.invoice = invoice;
                        return order;
                    }, splitOn: "COD_PRODUCT, COD_CLIENT, COD_SHIPPINGCOMPANY, COD_COMPANY, COD_COMPANY, NUMBER_NF", commandTimeout: 360);

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
                throw new Exception(@$"Jadlog - GetInvoicedOrders - Erro ao obter pedidos da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<Parameters> GetParameters(string doc_company, string tipo_servico)
        {
            var sql = $@"SELECT DISTINCT
                         A.CODIGO_CLIENTE AS COD_CLIENT,
                         A.MODALIDADE AS MODALITY,
                         A.TOKEN AS TOKEN,
                         LEFT(A.CONTA, 6) AS CONTA
                         FROM GENERAL..PARAMETROS_JADLOG A (NOLOCK)
                         WHERE 
                         A.CNPJ_EMP = '{doc_company}'
                         AND A.PRODUTO = '{tipo_servico}'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryFirstAsync<Parameters>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Jadlog - GetToken - Erro ao obter token da tabela GENERAL..PARAMETROS_JADLOG - {ex.Message}");
            }
        }
    }
}

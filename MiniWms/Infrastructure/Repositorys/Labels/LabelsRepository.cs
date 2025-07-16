using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;
using Order = BloomersMiniWmsIntegrations.Domain.Entities.Labels.Order;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public class LabelsRepository : ILabelsRepository
    {
        private readonly ISQLServerConnection _conn;

        public LabelsRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<Order> GetOrdersToPresent(string cnpj_emp, string serie, string nr_pedido)
        {
            var sql = $@"SELECT
                         DOCUMENTO AS NUMBER,
                         SUBSTRING (
	                         OBS,
	                         CHARINDEX(IIF(DOCUMENTO LIKE '%-VD%', 'Token do Troca Fácil:', 'Token:'), OBS),
	                         IIF(DOCUMENTO LIKE '%-VD%', 42, 28)
                         ) AS TOKEN,

                         COD_CLIENT,
                         REASON_CLIENT,
                         
                         COD_COMPANY,
                         REASON_COMPANY,
                         NAME_COMPANY,
                         DOC_COMPANY,
                         EMAIL_COMPANY,
                         ADDRESS_COMPANY,
                         STREET_NUMBER_COMPANY,
                         COMPLEMENT_ADDRESS_COMPANY,
                         NEIGHBORHOOD_COMPANY,
                         CITY_COMPANY,
                         UF_COMPANY,
                         ZIP_CODE_COMPANY,
                         FONE_COMPANY,
                         STATE_REGISTRATION_COMPANY,
                         
                         NUMBER_NF,
                         AMOUNT_NF,
                         KEY_NFE_NF,
                         XML_DISTRIBUITION_NF,
                         TYPE_NF,
                         SERIE_NF,
                         DATE_EMISSION_NF

                         FROM 
                         (
	                         SELECT
	                         DOCUMENTO,
							 NB_CODIGO_CLIENTE AS COD_CLIENT,
							 NB_RAZAO_CLIENTE AS REASON_CLIENT,
                         
							 NB_COD_REMETENTE AS COD_COMPANY,
							 NB_RAZAO_REMETENTE AS REASON_COMPANY,
							 NB_NOME_REMETENTE AS NAME_COMPANY,
							 NB_DOC_REMETENTE AS DOC_COMPANY,
							 NB_EMAIL_REMETENTE AS EMAIL_COMPANY,
							 NB_ENDERECO_REMETENTE AS ADDRESS_COMPANY,
							 NB_NUMERO_RUA_REMETENTE AS STREET_NUMBER_COMPANY,
							 NB_COMPLEMENTO_END_REMETENTE AS COMPLEMENT_ADDRESS_COMPANY,
							 NB_BAIRRO_REMETENTE AS NEIGHBORHOOD_COMPANY,
							 NB_CIDADE_REMETENTE AS CITY_COMPANY,
							 NB_UF_REMETENTE AS UF_COMPANY,
							 NB_CEP_REMETENTE AS ZIP_CODE_COMPANY,
							 NB_FONE_REMETENTE AS FONE_COMPANY,
							 NB_INSCRICAO_ESTADUAL_REMETENTE AS STATE_REGISTRATION_COMPANY,
                         
							 NF_SAIDA AS NUMBER_NF,
							 NB_VALOR_PEDIDO AS AMOUNT_NF,
							 CHAVE_NFE AS KEY_NFE_NF,
							 CAST(XML_FATURAMENTO AS VARCHAR(MAX)) AS XML_DISTRIBUITION_NF,
							 'NF' AS TYPE_NF,
							 (SELECT SUBSTRING ([XML_FATURAMENTO], CHARINDEX('<serie>', [XML_FATURAMENTO]) + 7, 1)) AS SERIE_NF,
							 (SELECT SUBSTRING ([XML_FATURAMENTO], CHARINDEX('<dhEmi>', [XML_FATURAMENTO]) + 7, 25)) AS DATE_EMISSION_NF,
	                         (SELECT SUBSTRING ([XML_FATURAMENTO], CHARINDEX('<infCpl>', CAST([XML_FATURAMENTO] AS VARCHAR(MAX))), CHARINDEX('</infCpl>', CAST([XML_FATURAMENTO] AS VARCHAR(MAX))))) AS OBS
	                         FROM GENERAL..IT4_WMS_DOCUMENTO (NOLOCK)
	                         WHERE 
	                         --NB_PARA_PRESENTE = 'S' 
	                         --AND 
                             NB_DOC_REMETENTE = '{cnpj_emp}'
                             AND SERIE = '{serie}'
                             AND DOCUMENTO = '{nr_pedido}'
	                         AND CHAVE_NFE IS NOT NULL
	                         AND (SELECT SUBSTRING ([XML_FATURAMENTO], CHARINDEX('<infCpl>', CAST([XML_FATURAMENTO] AS VARCHAR(MAX))), CHARINDEX('</infCpl>', CAST([XML_FATURAMENTO] AS VARCHAR(MAX))))) LIKE '%Token%'
                         ) AS PEDIDOS_PARA_PRESENTE";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Client, Company, Invoice, Order>(sql, (pedido, cliente, empresa, notaFiscal) =>
                    {
                        pedido.client = cliente;
                        pedido.company = empresa;
                        pedido.invoice = notaFiscal;
                        return pedido;
                    }, splitOn: "cod_client, cod_company, number_nf");

                    return result.First(); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Labels] - GetOrdersToPresent - Erro ao obter pedidos na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersToPrint(string cnpj_emp, string serie, string data_inicial, string data_final)
        {
            var sql = $@"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) AS NUMBER,
                        A.VOLUMES AS VOLUMES,
                        A.NB_CFOP_PEDIDO AS CFOP,
                        A.NB_ETIQUETA_IMPRESSA AS PRINTED,
                        A.NB_PARA_PRESENTE AS PRESENT,
                        A.SERIE AS SERIE,
                             
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<nProt>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 7, 15)) AS NPROT,
						(SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhRecbto>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 10, 25)) AS DATEPROT,
                             
						CASE
						WHEN (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<tpNF>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 6, 1)) = '1' THEN '1-SAIDA'
						WHEN (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<tpNF>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 6, 1)) = '0' THEN '0-ENTRADA'
						END AS TPNF,
                             
                        (SELECT DISTINCT TOP 1 Retorno FROM GENERAL..TotalExpressRegistroLog T1 WHERE T1.pedido = TRIM(A.DOCUMENTO) AND T1.retorno NOT LIKE '%erro%' AND T1.retorno NOT LIKE '%502 Bad Gateway%') AS RETURNSHIPPINGCOMPANY,
                             
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
                        A.NB_CEP_REMETENTE AS ZIP_CODE_COMPANY,
                        A.NB_FONE_REMETENTE AS FONE_COMPANY,
                        A.NB_INSCRICAO_ESTADUAL_REMETENTE AS STATE_REGISTRATION_COMPANY,
                              
                        A.NF_SAIDA AS NUMBER_NF,
                        A.NB_VALOR_PEDIDO AS AMOUNT_NF,
                        A.CHAVE_NFE AS KEY_NFE_NF,
                        CAST(A.XML_FATURAMENTO AS VARCHAR(MAX)) AS XML_DISTRIBUITION_NF,
                        'NF' AS TYPE_NF,
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<serie>', a.[XML_FATURAMENTO]) + 7, 1)) AS SERIE_NF,
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhEmi>', a.[XML_FATURAMENTO]) + 7, 25)) AS DATE_EMISSION_NF

                        FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                        WHERE
                        --A.NB_ETIQUETA_IMPRESSA = 'N' AND 
                        A.SERIE = '{serie}' 
                        AND A.CHAVE_NFE IS NOT NULL 
                        AND A.XML_FATURAMENTO IS NOT NULL
						AND LEFT(CONVERT(VARCHAR(MAX), A.XML_FATURAMENTO), 8) = '<nfeProc'
                        AND A.NB_DOC_REMETENTE = '{cnpj_emp}'
                        AND A.RETORNO >= '{data_inicial} 00:00:00'
                        AND A.RETORNO <= '{data_final} 23:59:59'
                        AND A.VOLUMES IS NOT NULL";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.QueryAsync<Order, Client, ShippingCompany, Company, Invoice, Order>(sql, (pedido, cliente, transportadora, empresa, notaFiscal) =>
                    {
                        pedido.client = cliente;
                        pedido.shippingCompany = transportadora;
                        pedido.company = empresa;
                        pedido.invoice = notaFiscal;
                        return pedido;
                    }, splitOn: "cod_client, cod_shippingCompany, cod_company, number_nf"); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Labels] - GetOrdersToPrint - Erro ao obter pedidos na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<Order> GetOrderToPrint(string cnpj_emp, string serie, string nr_pedido)
        {
            var sql = $@"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) AS NUMBER,
                        A.VOLUMES AS VOLUMES,
                        A.NB_CFOP_PEDIDO AS CFOP,
                        A.NB_ETIQUETA_IMPRESSA AS PRINTED,
                        A.NB_PARA_PRESENTE AS PRESENT,
                        A.SERIE AS SERIE,
                             
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<nProt>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 7, 15)) AS NPROT,
						(SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhRecbto>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 10, 25)) AS DATEPROT,
                             
						CASE
						WHEN (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<tpNF>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 6, 1)) = '1' THEN '1-SAIDA'
						WHEN (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<tpNF>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 6, 1)) = '0' THEN '0-ENTRADA'
						END AS TPNF,
                             
                        (SELECT DISTINCT TOP 1 Retorno FROM GENERAL..TotalExpressRegistroLog T1 WHERE T1.pedido = TRIM(A.DOCUMENTO) AND T1.retorno NOT LIKE '%erro%' AND T1.retorno NOT LIKE '%502 Bad Gateway%') AS RETURNSHIPPINGCOMPANY,
                             
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
                        A.NB_CEP_REMETENTE AS ZIP_CODE_COMPANY,
                        A.NB_FONE_REMETENTE AS FONE_COMPANY,
                        A.NB_INSCRICAO_ESTADUAL_REMETENTE AS STATE_REGISTRATION_COMPANY,
                              
                        A.NF_SAIDA AS NUMBER_NF,
                        A.NB_VALOR_PEDIDO AS AMOUNT_NF,
                        A.CHAVE_NFE AS KEY_NFE_NF,
                        CAST(A.XML_FATURAMENTO AS VARCHAR(MAX)) AS XML_DISTRIBUITION_NF,
                        'NF' AS TYPE_NF,
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<serie>', a.[XML_FATURAMENTO]) + 7, 1)) AS SERIE_NF,
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhEmi>', a.[XML_FATURAMENTO]) + 7, 25)) AS DATE_EMISSION_NF

                        FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                        WHERE
                        A.SERIE = '{serie}'
                        AND A.CHAVE_NFE IS NOT NULL 
                        AND A.XML_FATURAMENTO IS NOT NULL
						AND LEFT(CONVERT(VARCHAR(MAX), A.XML_FATURAMENTO), 8) = '<nfeProc'
                        AND A.NB_DOC_REMETENTE = '{cnpj_emp}' 
						AND A.DOCUMENTO = '{nr_pedido}'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Client, ShippingCompany, Company, Invoice, Order>(sql, (pedido, cliente, transportadora, empresa, notaFiscal) =>
                    {
                        pedido.client = cliente;
                        pedido.shippingCompany = transportadora;
                        pedido.company = empresa;
                        pedido.invoice = notaFiscal;
                        return pedido;
                    }, splitOn: "cod_client, cod_shippingCompany, cod_company, number_nf");
                    
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Labels] - GetOrderToPrint - Erro ao obter pedido: {nr_pedido} na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<int> UpdatePrintedFlag(string nr_pedido)
        {
            var sql = $@"UPDATE [GENERAL].[dbo].[IT4_WMS_DOCUMENTO] SET 
	                    NB_ETIQUETA_IMPRESSA = 'S'
                        WHERE TRIM(DOCUMENTO) = '{nr_pedido}'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.ExecuteAsync(sql); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Labels] - UpdatePrintedFlag - Erro ao atualizar status da etiqueta do pedido: {nr_pedido} como impressa na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }
    }
}

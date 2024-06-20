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
	                         FROM GENERAL..IT4_WMS_DOCUMENTO 
	                         WHERE 
	                         NB_PARA_PRESENTE = 'S' 
	                         AND NB_DOC_REMETENTE = '{cnpj_emp}'
                             AND SERIE = '{serie}'
                             AND DOCUMENTO = '{nr_pedido}'
	                         AND CHAVE_NFE IS NOT NULL
	                         AND (SELECT SUBSTRING ([XML_FATURAMENTO], CHARINDEX('<infCpl>', CAST([XML_FATURAMENTO] AS VARCHAR(MAX))), CHARINDEX('</infCpl>', CAST([XML_FATURAMENTO] AS VARCHAR(MAX))))) LIKE '%Token%'
                         ) AS PEDIDOS_PARA_PRESENTE";

            try
            {
                var result = await _conn.GetDbConnection().QueryAsync<Order, Client, Company, Invoice, Order>(sql, (pedido, cliente, empresa, notaFiscal) =>
                {
                    pedido.client = cliente;
                    pedido.company = empresa;
                    pedido.invoice = notaFiscal;
                    return pedido;
                }, splitOn: "cod_client, cod_company, number_nf");

                return result.First();
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Labels] - GetOrdersToPresent - Erro ao obter pedidos na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersToPrint(string cnpj_emp, string serie, string data_inicial, string data_final)
        {
            var sql = $@"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) as number,
                        A.VOLUMES as volumes,
                        A.NB_CFOP_PEDIDO as cfop,
                        A.NB_ETIQUETA_IMPRESSA as printed,
                        A.NB_PARA_PRESENTE as present,
                        A.SERIE as serie,
                             
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<nProt>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 7, 15)) as nProt,
						(SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhRecbto>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 10, 25)) as dateProt,
                             
						CASE
						WHEN (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<tpNF>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 6, 1)) = '1' THEN '1-SAIDA'
						WHEN (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<tpNF>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 6, 1)) = '0' THEN '0-ENTRADA'
						END AS tpNF,
                             
                        (SELECT DISTINCT TOP 1 Retorno FROM GENERAL..TotalExpressRegistroLog T1 WHERE T1.pedido = TRIM(A.DOCUMENTO) AND T1.retorno NOT LIKE '%erro%' AND T1.retorno NOT LIKE '%502 Bad Gateway%') as returnShippingCompany,
                             
                        A.NB_CODIGO_CLIENTE as cod_client,
                        A.NB_RAZAO_CLIENTE as reason_client,
                        A.NB_NOME_CLIENTE as name_client,
                        A.NB_DOC_CLIENTE as doc_client,
                        A.NB_EMAIL_CLIENTE as email_client,
                        A.NB_ENDERECO_CLIENTE as address_client,
                        A.NB_NUMERO_RUA_CLIENTE as street_number_client,
                        A.NB_COMPLEMENTO_END_CLIENTE as complement_address_client,
                        A.NB_BAIRRO_CLIENTE as neighborhood_client,
                        A.NB_CIDADE as city_client,
                        A.NB_ESTADO as uf_client,
                        TRIM(REPLACE(A.NB_CEP, '-', '')) as zip_code_client,
                        A.NB_FONE_CLIENTE as fone_client,
                        A.NB_INSCRICAO_ESTADUAL_CLIENTE as state_registration_client,
                        A.NB_INSCRICAO_MUNICIPAL_CLIENTE as municipal_registration_client,
                              
                        A.NB_TRANSPORTADORA as cod_shippingCompany,
                        A.NB_METODO_TRANSPORTADORA as metodo_shippingCompany,
                        A.NB_RAZAO_TRANSPORTADORA as reason_shippingCompany,
                        A.NB_NOME_TRANSPORTADORA as name_shippingCompany,
                        A.NB_DOC_TRANSPORTADORA as doc_shippingCompany,
                        A.NB_EMAIL_TRANSPORTADORA as email_shippingCompany,
                        A.NB_ENDERECO_TRANSPORTADORA as address_shippingCompany,
                        A.NB_NUMERO_RUA_TRANSPORTADORA as street_number_shippingCompany,
                        A.NB_COMPLEMENTO_END_TRANSPORTADORA as complement_address_shippingCompany,
                        A.NB_BAIRRO_TRANSPORTADORA as neighborhood_shippingCompany,
                        A.NB_CIDADE_TRANSPORTADORA as city_shippingCompany,
                        A.NB_UF_TRANSPORTADORA as uf_shippingCompany,
                        A.NB_CEP_TRANSPORTADORA as zip_code_shippingCompany,
                        A.NB_FONE_TRANSPORTADORA as fone_shippingCompany,
                        A.NB_INSCRICAO_ESTADUAL_TRANSPORTADORA as state_registration_shippingCompany,
                              
                        A.NB_COD_REMETENTE as cod_company,
                        A.NB_RAZAO_REMETENTE as reason_company,
                        A.NB_NOME_REMETENTE as name_company,
                        A.NB_DOC_REMETENTE as doc_company,
                        A.NB_EMAIL_REMETENTE as email_company,
                        A.NB_ENDERECO_REMETENTE as address_company,
                        A.NB_NUMERO_RUA_REMETENTE as street_number_company,
                        A.NB_COMPLEMENTO_END_REMETENTE as complement_address_company,
                        A.NB_BAIRRO_REMETENTE as neighborhood_company,
                        A.NB_CIDADE_REMETENTE as city_company,
                        A.NB_UF_REMETENTE as uf_company,
                        A.NB_CEP_REMETENTE as zip_code_company,
                        A.NB_FONE_REMETENTE as fone_company,
                        A.NB_INSCRICAO_ESTADUAL_REMETENTE as state_registration_company,
                              
                        A.NF_SAIDA as number_nf,
                        A.NB_VALOR_PEDIDO as amount_nf,
                        A.CHAVE_NFE as key_nfe_nf,
                        CAST(A.XML_FATURAMENTO AS VARCHAR(MAX)) as xml_distribuition_nf,
                        'NF' as type_nf,
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<serie>', a.[XML_FATURAMENTO]) + 7, 1)) as serie_nf,
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhEmi>', a.[XML_FATURAMENTO]) + 7, 25)) as date_emission_nf
                        FROM GENERAL..IT4_WMS_DOCUMENTO A
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
                return await _conn.GetDbConnection().QueryAsync<Order, Client, ShippingCompany, Company, Invoice, Order>(sql, (pedido, cliente, transportadora, empresa, notaFiscal) =>
                {
                    pedido.client = cliente;
                    pedido.shippingCompany = transportadora;
                    pedido.company = empresa;
                    pedido.invoice = notaFiscal;
                    return pedido;
                }, splitOn: "cod_client, cod_shippingCompany, cod_company, number_nf");
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Labels] - GetOrdersToPrint - Erro ao obter pedidos na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<Order> GetOrderToPrint(string cnpj_emp, string serie, string nr_pedido)
        {
            var sql = $@"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) as number,
                        A.VOLUMES as volumes,
                        A.NB_CFOP_PEDIDO as cfop,
                        A.NB_ETIQUETA_IMPRESSA as printed,
                        A.SERIE as serie,
                             
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<nProt>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 7, 15)) as nProt,
						(SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhRecbto>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 10, 25)) as dateProt,
                             
						CASE
						WHEN (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<tpNF>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 6, 1)) = '1' THEN '1-SAIDA'
						WHEN (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<tpNF>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 6, 1)) = '0' THEN '0-ENTRADA'
						END AS tpNF,
                             
                        (SELECT DISTINCT TOP 1 Retorno FROM GENERAL..TotalExpressRegistroLog T1 WHERE T1.pedido = TRIM(A.DOCUMENTO) AND T1.retorno NOT LIKE '%erro%' AND T1.retorno NOT LIKE '%502 Bad Gateway%') as returnShippingCompany,
                             
                        A.NB_CODIGO_CLIENTE as cod_client,
                        A.NB_RAZAO_CLIENTE as reason_client,
                        A.NB_NOME_CLIENTE as name_client,
                        A.NB_DOC_CLIENTE as doc_client,
                        A.NB_EMAIL_CLIENTE as email_client,
                        A.NB_ENDERECO_CLIENTE as address_client,
                        A.NB_NUMERO_RUA_CLIENTE as street_number_client,
                        A.NB_COMPLEMENTO_END_CLIENTE as complement_address_client,
                        A.NB_BAIRRO_CLIENTE as neighborhood_client,
                        A.NB_CIDADE as city_client,
                        A.NB_ESTADO as uf_client,
                        TRIM(REPLACE(A.NB_CEP, '-', '')) as zip_code_client,
                        A.NB_FONE_CLIENTE as fone_client,
                        A.NB_INSCRICAO_ESTADUAL_CLIENTE as state_registration_client,
                        A.NB_INSCRICAO_MUNICIPAL_CLIENTE as municipal_registration_client,
                              
                        A.NB_TRANSPORTADORA as cod_shippingCompany,
                        A.NB_METODO_TRANSPORTADORA as metodo_shippingCompany,
                        A.NB_RAZAO_TRANSPORTADORA as reason_shippingCompany,
                        A.NB_NOME_TRANSPORTADORA as name_shippingCompany,
                        A.NB_DOC_TRANSPORTADORA as doc_shippingCompany,
                        A.NB_EMAIL_TRANSPORTADORA as email_shippingCompany,
                        A.NB_ENDERECO_TRANSPORTADORA as address_shippingCompany,
                        A.NB_NUMERO_RUA_TRANSPORTADORA as street_number_shippingCompany,
                        A.NB_COMPLEMENTO_END_TRANSPORTADORA as complement_address_shippingCompany,
                        A.NB_BAIRRO_TRANSPORTADORA as neighborhood_shippingCompany,
                        A.NB_CIDADE_TRANSPORTADORA as city_shippingCompany,
                        A.NB_UF_TRANSPORTADORA as uf_shippingCompany,
                        A.NB_CEP_TRANSPORTADORA as zip_code_shippingCompany,
                        A.NB_FONE_TRANSPORTADORA as fone_shippingCompany,
                        A.NB_INSCRICAO_ESTADUAL_TRANSPORTADORA as state_registration_shippingCompany,
                              
                        A.NB_COD_REMETENTE as cod_company,
                        A.NB_RAZAO_REMETENTE as reason_company,
                        A.NB_NOME_REMETENTE as name_company,
                        A.NB_DOC_REMETENTE as doc_company,
                        A.NB_EMAIL_REMETENTE as email_company,
                        A.NB_ENDERECO_REMETENTE as address_company,
                        A.NB_NUMERO_RUA_REMETENTE as street_number_company,
                        A.NB_COMPLEMENTO_END_REMETENTE as complement_address_company,
                        A.NB_BAIRRO_REMETENTE as neighborhood_company,
                        A.NB_CIDADE_REMETENTE as city_company,
                        A.NB_UF_REMETENTE as uf_company,
                        A.NB_CEP_REMETENTE as zip_code_company,
                        A.NB_FONE_REMETENTE as fone_company,
                        A.NB_INSCRICAO_ESTADUAL_REMETENTE as state_registration_company,
                              
                        A.NF_SAIDA as number_nf,
                        A.NB_VALOR_PEDIDO as amount_nf,
                        A.CHAVE_NFE as key_nfe_nf,
                        CAST(A.XML_FATURAMENTO AS VARCHAR(MAX)) as xml_distribuition_nf,
                        'NF' as type_nf,
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<serie>', a.[XML_FATURAMENTO]) + 7, 1)) as serie_nf,
                        (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhEmi>', a.[XML_FATURAMENTO]) + 7, 25)) as date_emission_nf
                        FROM GENERAL..IT4_WMS_DOCUMENTO A
                        WHERE
                        A.SERIE = '{serie}'
                        AND A.CHAVE_NFE IS NOT NULL 
                        AND A.XML_FATURAMENTO IS NOT NULL
						AND LEFT(CONVERT(VARCHAR(MAX), A.XML_FATURAMENTO), 8) = '<nfeProc'
                        AND A.NB_DOC_REMETENTE = '{cnpj_emp}' 
						AND A.DOCUMENTO = '{nr_pedido}'";

            try
            {
                var result = await _conn.GetDbConnection().QueryAsync<Order, Client, ShippingCompany, Company, Invoice, Order>(sql, (pedido, cliente, transportadora, empresa, notaFiscal) =>
                {
                    pedido.client = cliente;
                    pedido.shippingCompany = transportadora;
                    pedido.company = empresa;
                    pedido.invoice = notaFiscal;
                    return pedido;
                }, splitOn: "cod_client, cod_shippingCompany, cod_company, number_nf");

                return result.First();
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
                        WHERE TRIM(Documento) = '{nr_pedido}'";

            try
            {
                return await _conn.GetDbConnection().ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Labels] - UpdatePrintedFlag - Erro ao atualizar status da etiqueta do pedido: {nr_pedido} como impressa na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }
    }
}

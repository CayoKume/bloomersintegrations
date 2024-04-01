using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;
using Order = BloomersWorkers.LabelsPrinter.Domain.Entities.Order;

namespace BloomersWorkers.LabelsPrinter.Infrastructure.Repositorys
{

    public class LabelsPrinterRepository : ILabelsPrinterRepository
    {
        private readonly ISQLServerConnection _conn;

        public LabelsPrinterRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public Task<IEnumerable<Order>> GetOrders()
        {
            try
            {
                var sql = $@"SELECT DISTINCT
                                     TRIM(A.DOCUMENTO) AS number,
                                     A.VOLUMES AS volumes,
                                     A.NB_CFOP_PEDIDO AS cfop,
                                     A.NB_ENTREGAR_PARA AS delivery_to,

                                     (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<nProt>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 7, 15)) AS nProt_order,
                                     (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhRecbto>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 10, 25)) AS dateProt_order,
                                     
                                     CASE
                                     WHEN (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<tpNF>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 6, 1)) = '1' THEN '1-SAIDA'
                                     WHEN (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<tpNF>', CAST(a.[XML_FATURAMENTO] AS VARCHAR(MAX))) + 6, 1)) = '0' THEN '0-ENTRADA'
                                     END AS tpNF_order,

                                     (SELECT DISTINCT TOP 1 Retorno FROM GENERAL..TotalExpressRegistroLog T1 WHERE T1.pedido = TRIM(A.DOCUMENTO) AND T1.retorno NOT LIKE '%erro%' AND T1.retorno NOT LIKE '%502 Bad Gateway%') AS _return,

                                     A.NB_CODIGO_CLIENTE AS cod_client,
                                     A.NB_RAZAO_CLIENTE AS reason_client,
                                     A.NB_NOME_CLIENTE AS name_client,
                                     A.NB_DOC_CLIENTE AS doc_client,
                                     A.NB_EMAIL_CLIENTE AS email_client,
                                     A.NB_ENDERECO_CLIENTE AS address_client,
                                     A.NB_NUMERO_RUA_CLIENTE AS street_number_client,
                                     A.NB_COMPLEMENTO_END_CLIENTE AS complement_address_client,
                                     A.NB_BAIRRO_CLIENTE AS neighborhood_client,
                                     A.NB_CIDADE AS city_client,
                                     A.NB_ESTADO AS uf_client,
                                     TRIM(REPLACE(A.NB_CEP, '-', '')) AS zip_code_client,
                                     A.NB_FONE_CLIENTE AS fone_client,
                                     A.NB_INSCRICAO_ESTADUAL_CLIENTE AS state_registration_client,
                                     A.NB_INSCRICAO_MUNICIPAL_CLIENTE AS municipal_registration_client,

                                     A.NB_TRANSPORTADORA AS cod_shippingCompany,
                                     A.NB_METODO_TRANSPORTADORA AS metodo_shippingCompany,
                                     A.NB_RAZAO_TRANSPORTADORA AS reason_shippingCompany,
                                     A.NB_NOME_TRANSPORTADORA AS name_shippingCompany,
                                     A.NB_DOC_TRANSPORTADORA AS doc_shippingCompany,
                                     A.NB_EMAIL_TRANSPORTADORA AS email_shippingCompany,
                                     A.NB_ENDERECO_TRANSPORTADORA AS address_shippingCompany,
                                     A.NB_NUMERO_RUA_TRANSPORTADORA AS street_number_shippingCompany,
                                     A.NB_COMPLEMENTO_END_TRANSPORTADORA AS complement_address_shippingCompany,
                                     A.NB_BAIRRO_TRANSPORTADORA AS neighborhood_shippingCompany,
                                     A.NB_CIDADE_TRANSPORTADORA AS city_shippingCompany,
                                     A.NB_UF_TRANSPORTADORA AS uf_shippingCompany,
                                     A.NB_CEP_TRANSPORTADORA AS zip_code_shippingCompany,
                                     A.NB_FONE_TRANSPORTADORA AS fone_shippingCompany,
                                     A.NB_INSCRICAO_ESTADUAL_TRANSPORTADORA AS state_registration_shippingCompany,

                                     A.NB_COD_REMETENTE AS cod_company,
                                     A.NB_RAZAO_REMETENTE AS reason_company,
                                     A.NB_NOME_REMETENTE AS name_company,
                                     A.NB_DOC_REMETENTE AS doc_company,
                                     A.NB_EMAIL_REMETENTE AS email_company,
                                     A.NB_ENDERECO_REMETENTE AS address_company,
                                     A.NB_NUMERO_RUA_REMETENTE AS street_number_company,
                                     A.NB_COMPLEMENTO_END_REMETENTE AS complement_address_company,
                                     A.NB_BAIRRO_REMETENTE AS neighborhood_company,
                                     A.NB_CIDADE_REMETENTE AS city_company,
                                     A.NB_UF_REMETENTE AS uf_company,
                                     A.NB_CEP_REMETENTE AS zip_code_company,
                                     A.NB_FONE_REMETENTE AS fone_company,
                                     A.NB_INSCRICAO_ESTADUAL_REMETENTE AS state_registration_company,

                                     A.NF_SAIDA AS number_nf,
                                     A.NB_VALOR_PEDIDO AS amount_nf,
                                     A.CHAVE_NFE AS key_nfe_nf,
                                     CAST(A.XML_FATURAMENTO AS VARCHAR(MAX)) AS xml_distribuition_nf,
                                     'NF' AS type_nf,
                                     (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<serie>', A.[XML_FATURAMENTO]) + 7, 1)) AS serie_nf,
                                     (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhEmi>', A.[XML_FATURAMENTO]) + 7, 25)) AS data_emission_nf
                                     FROM GENERAL..IT4_WMS_DOCUMENTO A
                                     WHERE
                                     A.DOCUMENTO IN ('MI-56786-01')
                                     --A.NB_ETIQUETA_IMPRESSA = 'N' 
                                     --AND A.SERIE = 'MI-' 
                                     --AND A.CHAVE_NFE IS NOT NULL 
                                     --AND A.XML_FATURAMENTO IS NOT NULL 
                                     --AND A.NB_DOC_REMETENTE = '38367316000199'
                                     --AND A.RETORNO > '2023-10-10'
                                     --AND A.VOLUMES IS NOT NULL";

                return _conn.GetIDbConnection().QueryAsync<Order, Client, ShippingCompany, Company, Invoice, Order>(sql, (order, client, shippingCompany, company, invoice) =>
                {
                    order.client = client;
                    order.shippingCompany = shippingCompany;
                    order.company = company;
                    order.invoice = invoice;
                    return order;
                }, splitOn: "cod_client, cod_shippingCompany, cod_company, number_nf");
            }
            catch (Exception ex)
            {
                throw new Exception($"GetOrders - Erro ao obter pedidos na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public Task UpdateStatus(string orderNumber)
        {
            var sql = $@"UPDATE [GENERAL].[dbo].[IT4_WMS_DOCUMENTO] SET 
	                         NB_ETIQUETA_IMPRESSA = 'S'
                             WHERE TRIM(Documento) = '{orderNumber}'";
            try
            {
                return _conn.GetIDbConnection().ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(@$"UpdateInvoice - Erro ao atualizar o status do pedido: {orderNumber} na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }
    }
}

using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersWorkersCore.Domain.Entities;
using BloomersWorkersCore.Infrastructure.Repositorys;
using Dapper;
using Order = BloomersWorkers.AuthorizeNFe.Domain.Entities.Order;

namespace BloomersWorkers.AuthorizeNFe.Infrastructure.Repositorys
{
    public class AuthorizeNFeRepository : IAuthorizeNFeRepository
    {
        private readonly IBloomersWorkersCoreRepository _bloomersWorkersCoreRepository;
        private readonly ISQLServerConnection _conn;

        public AuthorizeNFeRepository(ISQLServerConnection conn, IBloomersWorkersCoreRepository bloomersWorkersCoreRepository) =>
            (_conn, _bloomersWorkersCoreRepository) = (conn, bloomersWorkersCoreRepository);

        public async Task<MicrovixUser> GetMicrovixUser(string gabot)
        {
            try
            {
                return await _bloomersWorkersCoreRepository.GetMicrovixUser(gabot);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Order>> GetPendingNFesFromB2CConsultaNFe()
        {
            var sql = @$"SELECT DISTINCT 
	                            B.ORDER_ID AS NUMBER, 
	                            C.DESCRICAO AS DESCRIPTION,
	                            D.CNPJ_EMP AS DOC_COMPANY, 
	                            A.DOCUMENTO AS NUMBER_NF, 
	                            A.DATA_EMISSAO AS DATE_EMISSION_NF, 
	                            A.CHAVE_NFE AS KEY_NFE_NF, 
	                            A.[XML] AS XML_NF
                            FROM BLOOMERS_LINX..B2CCONSULTANFE_TRUSTED A (NOLOCK)
	                            JOIN BLOOMERS_LINX..B2CCONSULTAPEDIDOS_TRUSTED B (NOLOCK) ON A.ID_PEDIDO = B.ID_PEDIDO
	                            JOIN BLOOMERS_LINX..B2CCONSULTANFESITUACAO_TRUSTED C (NOLOCK) ON A.SITUACAO = C.ID_NFE_SITUACAO
	                            JOIN BLOOMERS_LINX..LINXLOJAS_TRUSTED D (NOLOCK) ON B.EMPRESA = D.EMPRESA
	                            JOIN GENERAL..IT4_WMS_DOCUMENTO E (NOLOCK) ON REPLACE(B.ORDER_ID, '-CANCELLED', '') = TRIM(E.DOCUMENTO)
                            WHERE
                                --A.CHAVE_NFE IN ('')
	                            A.SITUACAO NOT IN (1,2,3,4,8,10,11,12,13,14,15,16) AND 
	                            A.SITUACAO IN (5,6,7,9) AND 
	                            B.ORDER_ID NOT LIKE ('%-CANCELLED%') AND 
	                            A.DATA_EMISSAO > '2023-10-01' AND
	                            E.CHAVE_NFE IS NULL AND
	                            E.XML_FATURAMENTO IS NULL AND
	                            E.NF_SAIDA IS NULL";

            try
            {
                var result = await _conn.GetDbConnection().QueryAsync<Order, Company, Invoice, Order>(sql, (order, company, invoice) =>
                {
                    order.company = company;
                    order.invoice = invoice;

                    return order;
                }, splitOn: "doc_company, number_nf");

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($@" - GetPendingDocumentsFromB2CConsultaNFe - Erro ao obter notas fiscais com status nao autorizadas da tabela B2CCONSULTANFE_TRUSTED  - {ex.Message}");
            }
        }

        public async Task<List<Order>> GetPendingNFesFromLinxXMLDocumentos()
        {
            var sql = @$"SELECT DISTINCT 
	                            E.DOCUMENTO AS NUMBER, 
	                            C.DESCRICAO AS DESCRIPTION,
	                            A.CNPJ_EMP AS DOC_COMPANY, 
	                            B.DOCUMENTO AS NUMBER_NF, 
	                            B.DATA_EMISSAO AS DATE_EMISSION_NF, 
	                            B.CHAVE_NFE AS KEY_NFE_NF, 
	                            B.[XML] AS XML_NF
                            FROM BLOOMERS_LINX..LINXPEDIDOSVENDA_TRUSTED A (NOLOCK) 
								JOIN BLOOMERS_LINX..LINXXMLDOCUMENTOS_TRUSTED B (NOLOCK) ON IIF(A.NF_GERADA LIKE '%,%', (SELECT TOP 1 VALUE FROM STRING_SPLIT(A.NF_GERADA, ',') ORDER BY VALUE ASC), A.NF_GERADA) = B.DOCUMENTO
	                            JOIN BLOOMERS_LINX..B2CCONSULTANFESITUACAO_TRUSTED C (NOLOCK) ON B.SITUACAO = C.ID_NFE_SITUACAO
	                            JOIN GENERAL..IT4_WMS_DOCUMENTO E (NOLOCK) ON A.COD_PEDIDO = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(TRIM(E.DOCUMENTO), 'MI-VF', ''), 'MI-LJ', ''), 'MI-VD', ''), 'OA-VF', ''), 'OA-LJ', ''), 'OA-VD', '')
                            WHERE
                                --B.CHAVE_NFE IN ('') AND
	                            B.SITUACAO NOT IN (1,2,3,4,8,10,11,12,13,14,15,16) AND 
	                            B.SITUACAO IN (5,6,7,9) AND 
	                            B.DATA_EMISSAO > '2023-10-01' AND
	                            E.CHAVE_NFE IS NULL AND
	                            E.XML_FATURAMENTO IS NULL AND
	                            E.NF_SAIDA IS NULL AND
								(E.DOCUMENTO LIKE '%-VD%' OR E.DOCUMENTO LIKE '%-LJ%') AND
								E.SERIE NOT LIKE 'MX-' AND
                                B.DOCUMENTO != 0";

            try
            {
                var result = await _conn.GetDbConnection().QueryAsync<Order, Company, Invoice, Order>(sql, (order, company, invoice) =>
                {
                    order.company = company;
                    order.invoice = invoice;

                    return order;
                }, splitOn: "doc_company, number_nf");

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($@" - GetPendingDocumentsFromB2CConsultaNFe - Erro ao obter notas fiscais com status nao autorizadas da tabela B2CCONSULTANFE_TRUSTED  - {ex.Message}");
            }
        }
    }
}

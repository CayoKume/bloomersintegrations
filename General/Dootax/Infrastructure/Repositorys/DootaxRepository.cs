using BloomersGeneralIntegrations.Dootax.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;

namespace BloomersGeneralIntegrations.Dootax.Infrastructure.Repositorys
{
    public class DootaxRepository : IDootaxRepository
    {
        private readonly ISQLServerConnection _conn;

        public DootaxRepository(ISQLServerConnection conn) =>
            _conn = conn;

        public async Task<IEnumerable<XML>> GetXMLs()
        {
            var sql = @$"SELECT
                            A.NF_SAIDA as Documento,
                            A.NB_DOC_REMETENTE as CNPJCPF,
                            (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<serie>', a.[XML_FATURAMENTO]) + 7, 1)) as Serie,
                            A.CHAVE_NFE as ChaveNfe,
                            A.XML_FATURAMENTO as DsXml
                        FROM 
                        GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                        LEFT JOIN GENERAL..DootaxXMLDocumento_log B (NOLOCK) on A.CHAVE_NFE = B.CHAVE_NFE
                        WHERE
                            B.CHAVE_NFE IS NULL
                            --AND (NB_COD_REMETENTE = 1 OR NB_COD_REMETENTE = 5) -- SOMENTE MISHA E OPEN ECOM
                            AND CONVERT(DATE, (SELECT SUBSTRING (A.[XML_FATURAMENTO], CHARINDEX('<dhEmi>', a.[XML_FATURAMENTO]) + 7, 25))) > '2023-11-30'
                            AND A.NB_UF_REMETENTE != A.NB_ESTADO
                            AND A.CHAVE_NFE IS NOT NULL
                            AND A.NF_SAIDA IS NOT NULL
                            AND A.XML_FATURAMENTO IS NOT NULL
                            AND A.DOCUMENTO NOT LIKE '%-LJ%'
                            ORDER BY NF_SAIDA ASC";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryAsync<XML>(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Dootax - GetXMLs - Erro ao obter xmls da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task InsertSendXMLOk_Log(string cnpjcpf, string documento, string serie, string chavenfe)
        {
            var sql = @$"INSERT INTO GENERAL..DootaxXMLDocumento_log 
                                (cnpj_emp, documento, serie, chave_nfe, dt_insert)
                          VALUES 
                                (@cnpj_emp, @documento, @serie, @chave_nfe, @dt_insert)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, new
                    {
                        cnpj_emp = cnpjcpf,
                        documento = documento,
                        serie = serie,
                        chave_nfe = chavenfe,
                        dt_insert = DateTime.Now
                    }, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Dootax - GetXMLs - Erro ao obter xmls da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }
    }
}

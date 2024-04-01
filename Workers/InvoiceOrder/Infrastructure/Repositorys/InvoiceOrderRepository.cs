using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersWorkersCore.Domain.Entities;
using Dapper;
using Order = BloomersWorkers.InvoiceOrder.Domain.Entities.Order;

namespace BloomersWorkers.InvoiceOrder.Infrastructure.Repositorys
{
    public class InvoiceOrderRepository : IInvoiceOrderRepository
    {
        private readonly ISQLServerConnection _conn;

        public InvoiceOrderRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<IEnumerable<Order>> GetOrdersFromIT4(string botName)
        {
            try
            {
                string? sql = String.Empty;
                if (botName.Contains("Gabot"))
                {
                    string[] _subs = botName.Substring(botName.Length - 5, 5).Split(" e ");

                    sql = $@"SELECT DISTINCT 
                             TRIM(A.DOCUMENTO) AS NUMBER,
                             A.NB_TENTATIVAS_DE_FATURAMENTO AS INVOICE_ATTEMPT,
                             ISNULL (A.VOLUMES, '1') AS VOLUMES,
                             A.NB_DOC_REMETENTE AS DOC_COMPANY,
                             A.NB_TRANSPORTADORA AS COD_SHIPPINGCOMPANY
                             FROM [GENERAL].[dbo].[IT4_WMS_DOCUMENTO] A
                             WHERE 
                             --A.DOCUMENTO IN ('')
                             ((SELECT TRIM(RIGHT(A.IDCONTROLE, 1))) = '{_subs[0]}' OR (SELECT TRIM(RIGHT(A.IDCONTROLE, 1))) = '{_subs[1]}') AND 
                             A.SERIE = 'OA-'
                             AND A.RETORNO IS NOT NULL
                             AND A.CHAVE_NFE IS NULL
                             AND A.NF_SAIDA IS NULL
                             AND A.XML_FATURAMENTO IS NULL
                             AND A.ORIGEM = 'P'
                             AND A.NB_DOC_REMETENTE = '42538267000268'
                             AND A.CANCELADO IS NULL
                             AND A.NB_TENTATIVAS_DE_FATURAMENTO <= 10
                             AND NOT EXISTS (SELECT 0 FROM [GENERAL].[dbo].[IT4_WMS_DOCUMENTO_ITEM] B WHERE A.IDCONTROLE = B.IDCONTROLE AND B.QTDE != B.QTDERETORNO)";
                }
                else if (botName.Contains("Vanabot"))
                {
                    sql = $@"SELECT DISTINCT 
                             TRIM(A.DOCUMENTO) AS NUMBER,
                             A.NB_TENTATIVAS_DE_FATURAMENTO AS INVOICE_ATTEMPT,
                             ISNULL (A.VOLUMES, '1') AS VOLUMES,
                             A.NB_DOC_REMETENTE AS DOC_COMPANY,
                             A.NB_TRANSPORTADORA AS COD_SHIPPINGCOMPANY
                             FROM [GENERAL].[dbo].[IT4_WMS_DOCUMENTO] A
                             WHERE 
                             --A.DOCUMENTO IN ()
                             (SELECT TRIM(RIGHT(A.IDCONTROLE, 1))) = '{botName.Substring(botName.Length - 1, 1)}' AND
                             A.SERIE = 'MI-'
                             AND A.RETORNO IS NOT NULL
                             AND A.CHAVE_NFE IS NULL
                             AND A.NF_SAIDA IS NULL
                             AND A.XML_FATURAMENTO IS NULL
                             AND A.ORIGEM = 'P'
                             AND A.NB_DOC_REMETENTE IN ('38367316000199', '38367316000865')
                             AND A.CANCELADO IS NULL
                             AND A.NB_TENTATIVAS_DE_FATURAMENTO <= 10
                             AND NOT EXISTS (SELECT 0 FROM [GENERAL].[dbo].[IT4_WMS_DOCUMENTO_ITEM] B WHERE A.IDCONTROLE = B.IDCONTROLE AND B.QTDE != B.QTDERETORNO)";
                }

                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.QueryAsync<Order, Company, ShippingCompany, Order>(sql, (order, company, shippingCompany) =>
                    {
                        order.company = company;
                        order.shippingCompany = shippingCompany;
                        return order;
                    }, splitOn: "doc_company, cod_shippingcompany");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"GetOrdersFromIT4 - Erro ao obter pedidos para serem faturados da tabela IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<int> UpdateInvoiceAttemptIT4(string number, int invoice_attempt)
        {
            try
            {
                var sql = $@"UPDATE GENERAL..IT4_WMS_DOCUMENTO SET 
                             NB_TENTATIVAS_DE_FATURAMENTO = {invoice_attempt}
                             WHERE DOCUMENTO = '{number}'";

                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.ExecuteAsync(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"UpdateInvoiceAttemptIT4 - Erro ao atualizar registro do pedido na tabela IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<MicrovixUser> GetMicrovixUser(string gabot)
        {
            try
            {
                var sql = $@"SELECT 
                         usuario, senha FROM GENERAL..VoloInvoiceUsuariosMicrovix
                         WHERE gabot = '{gabot}'";

                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<MicrovixUser>(sql);
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"GetMicrovixUser - Erro ao obter usuario para o bot na tabela GENERAL..VoloInvoiceUsuariosMicrovix - {ex.Message}");
            }
        }
    }
}

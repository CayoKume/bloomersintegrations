using BloomersGeneralIntegrations.Mobsim.Domain.Entities;
using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;

namespace BloomersGeneralIntegrations.Mobsim.Infrastructure.Repositorys
{
    public class MobsimRepository : IMobsimRepository
    {
        private readonly ISQLServerConnection _conn;

        public MobsimRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<Domain.Entities.Client> GetClient(string cod_client)
        {
            var sql = $@"SELECT 
                            cod_cliente as CodCliente,
                            razao_cliente as NomeCliente,
                            CASE
                                WHEN cel_cliente != '' THEN TRIM(Replace(Replace(Replace(Replace(cel_cliente, '-', ''), '(', ''), ')', ''), ' ', ''))
                                WHEN fone_cliente != '' THEN TRIM(Replace(Replace(Replace(Replace(fone_cliente, '-', ''), '(', ''), ')', ''), ' ', ''))
                                ELSE ''
                            END as CelularCliente
                            FROM BLOOMERS_LINX..LinxClientesFornec_trusted
                            WHERE cod_cliente = @codCliente";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    var result = await conn.QueryAsync<Domain.Entities.Client>(sql, new { cod_client = cod_client }, commandTimeout: 360);
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Mobsim - GetCliente - Erro ao obter cliente da tabela BLOOMERS_LINX..LinxClientesFornec_trusted - {ex.Message}");
            }
        }

        public async Task<List<IT4_WMS_Documento>> GetDeliveredOrders()
        {
            var sql = $@"SELECT 
	                        TRIM(DOCUMENTO) AS Documento,
	                        Cliente_Ou_Fornecedor AS CodCliente,
	                        NB_DESCRICAO_ULTIMO_STATUS AS Descricao,
	                        NB_DATA_ULTIMO_STATUS AS DataUltimoStatus,
	                        TIPO AS Tipo,
                            NB_CODIGO_RASTREIO AS Rastreio,
							NB_TRANSPORTADORA AS Transportadora
                            FROM 
                            GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                            WHERE 
                            (XML_FATURAMENTO IS NOT NULL 
                            AND CHAVE_NFE IS NOT NULL
                            AND NF_SAIDA IS NOT NULL)
                            AND NB_DATA_ULTIMO_STATUS IS NOT NULL 
                            AND NB_DESCRICAO_ULTIMO_STATUS IS NOT NULL
                            AND NB_DATA_ENTREGA_REALIZADA IS NOT NULL
                            AND TIPO = 'ECOM'
                            AND SERIE != 'MX-'
						    AND (NB_DESCRICAO_ULTIMO_STATUS = '1-ENTREGA REALIZADA' 
						    OR NB_DESCRICAO_ULTIMO_STATUS = 'Comprovante registrado'
						    OR NB_DESCRICAO_ULTIMO_STATUS = '4250-Entrega registrada via RT'
						    OR NB_DESCRICAO_ULTIMO_STATUS = '4300-Entrega registrada')
                            AND DATA > '2023-12-01'
						    AND (NB_TRANSPORTADORA = 7601 OR NB_TRANSPORTADORA = 18035)
							AND EXISTS (SELECT 0 FROM GENERAL..MOBSIMHISTORICO B (NOLOCK) WHERE A.DOCUMENTO = B.PEDIDO AND B.ENTREGUE = 0);";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return (List<IT4_WMS_Documento>)await conn.QueryAsync<IT4_WMS_Documento>(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Mobsim - GetPedidosEntregues - Erro ao obter pedidos entregues da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<List<IT4_WMS_Documento>> GetInvoicedOrders()
        {
            var sql = $@"SELECT
                         TRIM(DOCUMENTO) AS Documento,
                         Cliente_Ou_Fornecedor AS CodCliente,
                         NB_DESCRICAO_ULTIMO_STATUS AS Descricao,
                         NB_DATA_ULTIMO_STATUS AS DataUltimoStatus,
                         TIPO AS Tipo,
                         NB_CODIGO_RASTREIO AS Rastreio,
                         NB_TRANSPORTADORA AS Transportadora
                         FROM 
                         GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                         WHERE 
                         (XML_FATURAMENTO IS NOT NULL 
                         AND CHAVE_NFE IS NOT NULL
                         AND NF_SAIDA IS NOT NULL)
                         AND TIPO = 'ECOM'
                         AND SERIE != 'MX-'
                         AND DATA > '2024-01-17'
                         AND (NB_TRANSPORTADORA = 7601 OR NB_TRANSPORTADORA = 18035)
                         AND NOT EXISTS (SELECT 0 FROM GENERAL..MOBSIMHISTORICO B (NOLOCK) WHERE A.DOCUMENTO = B.PEDIDO);";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return (List<IT4_WMS_Documento>)await conn.QueryAsync<IT4_WMS_Documento>(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Mobsim - GetPedidosFaturados - Erro ao obter pedidos faturados da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<List<IT4_WMS_Documento>> GetShippedOrders()
        {
            var sql = $@"SELECT 
	                        TRIM(DOCUMENTO) AS Documento,
	                        Cliente_Ou_Fornecedor AS CodCliente,
	                        NB_DESCRICAO_ULTIMO_STATUS AS Descricao,
	                        NB_DATA_ULTIMO_STATUS AS DataUltimoStatus,
	                        TIPO AS Tipo,
                            NB_CODIGO_RASTREIO AS Rastreio,
							NB_TRANSPORTADORA AS Transportadora
                            FROM 
                            GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                            WHERE 
                            (XML_FATURAMENTO IS NOT NULL 
                            AND CHAVE_NFE IS NOT NULL 
                            AND NF_SAIDA IS NOT NULL)
                            AND NB_DATA_ULTIMO_STATUS IS NOT NULL 
                            AND NB_DESCRICAO_ULTIMO_STATUS IS NOT NULL
                            AND NB_DATA_ENTREGA_REALIZADA IS NULL
                            AND TIPO = 'ECOM'
                            AND SERIE != 'MX-'
						    AND (NB_DESCRICAO_ULTIMO_STATUS like '%Entrega em andamento (na rua)%' 
						    OR NB_DESCRICAO_ULTIMO_STATUS = '104-PROCESSO DE ENTREGA' 
						    OR NB_DESCRICAO_ULTIMO_STATUS = '105-EM ROTA')
						    AND DATA > '2023-12-01'
						    AND (NB_TRANSPORTADORA = 7601 OR NB_TRANSPORTADORA = 18035)
							AND EXISTS (SELECT 0 FROM GENERAL..MOBSIMHISTORICO B (NOLOCK) WHERE A.DOCUMENTO = B.PEDIDO AND B.ENVIADO = 0);";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return (List<IT4_WMS_Documento>)await conn.QueryAsync<IT4_WMS_Documento>(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Mobsim - GetPedidosExpedidos - Erro ao obter pedidos expedidos da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<MobsimHistoricoModel> HasLogInMobsimHistoric(string order)
        {
            var sql = $@"SELECT * FROM GENERAL..MobsimHistorico WHERE PEDIDO = @pedido";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    var result = await conn.QueryAsync<MobsimHistoricoModel>(sql, new { pedido = order }, commandTimeout: 360);

                    if (result.Count() > 0)
                        return result.First();
                    else
                        return new MobsimHistoricoModel();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Mobsim - HasLogInMobsimHistorico - Erro ao obter log da tabela GENERAL..MobsimHistorico - {ex.Message}");
            }
        }

        public async Task InsertMobsimHistoric(Guid idMobsim, string order, string client)
        {
            var sql = $@"insert into GENERAL..MobsimHistorico (ID_MOBSIM, PEDIDO, CLIENTE, FATURADO, ENVIADO, ENTREGUE, DATA_ULTIMA_MENSAGEM) 
                                    values (@idMobsim, @pedido, @cliente, 1, 0, 0, getdate())";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, new { idMobsim = idMobsim, pedido = order, cliente = client }, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Mobsim - InsertMobsimHistorico - Erro ao inserir log na tabela GENERAL..MobsimHistorico - {ex.Message}");
            }
        }

        public async Task UpdateStatusMobsimHistoric(string order, bool sended, bool delivered)
        {
            var sql = String.Empty;

            if (sended is true)
                sql = @"UPDATE GENERAL..MobsimHistorico SET ENVIADO = 1 WHERE PEDIDO = @pedido";
            else if (delivered is true)
                sql = @"UPDATE GENERAL..MobsimHistorico SET ENTREGUE = 1 WHERE PEDIDO = @pedido";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, new { pedido = order }, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Mobsim - UpdateStatusMobsimHistorico - Erro ao atualizar log na tabela GENERAL..MobsimHistorico - {ex.Message}");
            }
        }
    }
}

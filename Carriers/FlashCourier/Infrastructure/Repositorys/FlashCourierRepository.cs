using Order = BloomersCarriersIntegrations.FlashCourier.Domain.Entities.Order;
using BloomersIntegrationsCore.Domain.Entities;
using BloomersCarriersIntegrations.FlashCourier.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;

namespace BloomersCarriersIntegrations.FlashCourier.Infrastructure.Repositorys
{
    public class FlashCourierRepository : IFlashCourierRepository
    {
        private readonly ISQLServerConnection _conn;

        public FlashCourierRepository(ISQLServerConnection conn) =>
            _conn = conn;

        public async Task GenerateRequestLog(string orderNumber, string request)
        {
            var sql = $@"INSERT INTO [GENERAL].[dbo].FlashCourierRequestLog (Pedido, DataEnvio, Request) 
                         VALUES(@OrderNumber, GETDATE(), @request)";
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    await conn.ExecuteAsync(
                        sql,
                        new
                        {
                            OrderNumber = orderNumber,
                            Request = request
                        },
                        commandTimeout: 360
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GenerateRequestLog - Erro ao inserir registro: {orderNumber} na tabela GENERAL..FlashCourierRequestLog - {ex.Message}");
            }
        }

        public async Task GenerateSucessLog(string orderNumber, string senderID, string _return, string statusFlash, string keyNFe)
        {
            var sql = $@"INSERT INTO [GENERAL].[dbo].FlashCourierRegistroLog (Pedido, DataEnvio, Retorno, Remetente, StatusFlash, ChaveNFe) 
                         VALUES(@OrderNumber, GETDATE(), @Return, @SenderID, @StatusFlash, @KeyNFe)";
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    await conn.ExecuteAsync(
                        sql,
                        new { 
                            OrderNumber = orderNumber, 
                            Return = _return, 
                            SenderID = senderID, 
                            StatusFlash = statusFlash, 
                            KeyNFe = keyNFe 
                        },
                        commandTimeout: 360
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GenerateSucessLog - Erro ao inserir registro: {orderNumber} na tabela GENERAL..FlashCourierRegistroLog - {ex.Message}");
            }
        }

        public async Task<Order> GetInvoicedOrder(string orderNumber)
        {
            try
            {
                string sql = $@"SELECT DISTINCT
	                            TRIM(A.DOCUMENTO) AS number,
	                            SUM(C.PESO_BRUTO) AS weight,
	                            B.QTDERETORNO AS quantity,

	                            A.NB_CODIGO_CLIENTE AS cod_client,
	                            A.NB_DOC_CLIENTE AS doc_client,
	                            A.NB_RAZAO_CLIENTE AS reason_client,
	                            A.NB_EMAIL_CLIENTE AS email_client,
	                            A.NB_ENDERECO_CLIENTE AS address_client,
	                            A.NB_NUMERO_RUA_CLIENTE AS street_number_client,
	                            A.NB_COMPLEMENTO_END_CLIENTE AS complement_address_client,
	                            A.NB_BAIRRO_CLIENTE AS neighborhood_client,
	                            A.NB_CIDADE AS city_client,
	                            A.NB_ESTADO AS uf_client,
	                            A.NB_CEP AS zip_code_client,
	                            A.NB_FONE_CLIENTE AS fone_client,
	                            A.NB_INSCRICAO_ESTADUAL_CLIENTE AS state_registration_client,
	
	                            A.NB_DOC_REMETENTE AS doc_company,
	
	                            A.NF_SAIDA AS number_nf,
	                            A.NB_VALOR_PEDIDO AS amount_nf,
	                            A.CHAVE_NFE AS key_nfe_nf
                            FROM 
	                            GENERAL..IT4_WMS_DOCUMENTO (NOLOCK) A
                            JOIN 
	                            GENERAL..IT4_WMS_DOCUMENTO_ITEM B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE 
                            JOIN 
	                            BLOOMERS_LINX..LINXPRODUTOS_TRUSTED C (NOLOCK) ON B.CODIGO_BARRA = C.COD_PRODUTO
                            WHERE 
	                            A.NB_TRANSPORTADORA = '18035'
	                            AND A.DOCUMENTO = '{orderNumber}'
	                            AND A.CHAVE_NFE IS NOT NULL
                            GROUP BY 
	                            A.DOCUMENTO,
                                A.NB_CODIGO_CLIENTE,
                                A.NB_DOC_REMETENTE,
                                A.NF_SAIDA,
                                A.CHAVE_NFE,
	                            A.NB_VALOR_PEDIDO,
	                            A.NB_EMAIL_CLIENTE,
                                A.NB_RAZAO_CLIENTE,
                                A.NB_ENDERECO_CLIENTE,
                                A.NB_BAIRRO_CLIENTE,
                                A.NB_NUMERO_RUA_CLIENTE,
                                A.NB_COMPLEMENTO_END_CLIENTE,
                                A.NB_CIDADE,
                                A.NB_ESTADO,
                                A.NB_CEP,
                                A.NB_FONE_CLIENTE,
                                A.NB_DOC_CLIENTE,
	                            A.NB_INSCRICAO_ESTADUAL_CLIENTE,
                                B.QTDERETORNO";

                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Client, Company, Invoice, Order>(sql, (order, client, company, invoice) =>
                    {
                        order.client = client;
                        order.company = company;
                        order.invoice = invoice;
                        return order;
                    }, splitOn: "cod_client, doc_company, number_nf", commandTimeout: 360);

                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GetInvoicedOrder - Erro ao obter pedido: {orderNumber} da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<IEnumerable<Order>> GetInvoicedOrders()
        {
            try
            {
                string sql = @"SELECT DISTINCT
	                            TRIM(A.DOCUMENTO) AS number,
	                            SUM(C.PESO_BRUTO) AS weight,
	                            B.QTDERETORNO AS quantity,

	                            A.NB_CODIGO_CLIENTE AS cod_client,
	                            A.NB_DOC_CLIENTE AS doc_client,
	                            A.NB_RAZAO_CLIENTE AS reason_client,
	                            A.NB_EMAIL_CLIENTE AS email_client,
	                            A.NB_ENDERECO_CLIENTE AS address_client,
	                            A.NB_NUMERO_RUA_CLIENTE AS street_number_client,
	                            A.NB_COMPLEMENTO_END_CLIENTE AS complement_address_client,
	                            A.NB_BAIRRO_CLIENTE AS neighborhood_client,
	                            A.NB_CIDADE AS city_client,
	                            A.NB_ESTADO AS uf_client,
	                            A.NB_CEP AS zip_code_client,
	                            A.NB_FONE_CLIENTE AS fone_client,
	                            A.NB_INSCRICAO_ESTADUAL_CLIENTE AS state_registration_client,
	
	                            A.NB_DOC_REMETENTE as doc_company,
	
	                            A.NF_SAIDA AS number_nf,
	                            A.NB_VALOR_PEDIDO AS amount_nf,
	                            A.CHAVE_NFE AS key_nf
                            FROM 
	                            GENERAL..IT4_WMS_DOCUMENTO (NOLOCK) A
                            JOIN 
	                            GENERAL..IT4_WMS_DOCUMENTO_ITEM B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE 
                            JOIN 
	                            BLOOMERS_LINX..LINXPRODUTOS_TRUSTED C (NOLOCK) ON B.CODIGO_BARRA = C.COD_PRODUTO
                            LEFT JOIN 
	                            GENERAL..FLASHCOURIERREGISTROLOG D (NOLOCK) ON D.CHAVENFE = A.CHAVE_NFE
                            WHERE 
	                            A.NB_TRANSPORTADORA = '18035'
                                --TESTE
	                            --AND A.DOCUMENTO IN ('')
	                            --AND D.PEDIDO IS NULL
	                            AND A.CHAVE_NFE IS NOT NULL
	                            AND A.[DATA] > GETDATE() - 15
                            GROUP BY 
	                            A.DOCUMENTO,
                                A.NB_CODIGO_CLIENTE,
                                A.NB_DOC_REMETENTE,
                                A.NF_SAIDA,
                                A.CHAVE_NFE,
	                            A.NB_VALOR_PEDIDO,
	                            A.NB_EMAIL_CLIENTE,
                                A.NB_RAZAO_CLIENTE,
                                A.NB_ENDERECO_CLIENTE,
                                A.NB_BAIRRO_CLIENTE,
                                A.NB_NUMERO_RUA_CLIENTE,
                                A.NB_COMPLEMENTO_END_CLIENTE,
                                A.NB_CIDADE,
                                A.NB_ESTADO,
                                A.NB_CEP,
                                A.NB_FONE_CLIENTE,
                                A.NB_DOC_CLIENTE,
	                            A.NB_INSCRICAO_ESTADUAL_CLIENTE,
                                B.QTDERETORNO";

                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.QueryAsync<Order, Client, Company, Invoice, Order>(sql, (order, client, company, invoice) =>
                    {
                        order.client = client;
                        order.company = company;
                        order.invoice = invoice;
                        return order;
                    }, splitOn: "cod_client, doc_company, number_nf", commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GetInvoicedOrders - Erro ao obter pedidos da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<IEnumerable<FlashCourierRegisterLog>> GetShippedOrders()
        {
            var query = @"SELECT DISTINCT
                            A.PEDIDO AS orderNumber,
                            A.DATAENVIO AS shippingDate,
                            A.RETORNO AS _return,
                            A.STATUSECOM AS statusEcom,
                            A.REMETENTE AS sender,
                            A.STATUSFLASH AS statusFlash,
                            A.CHAVENFE AS keyNFe,
                            C.NB_DOC_REMETENTE AS doc_company
                            FROM [GENERAL].[dbo].FLASHCOURIERREGISTROLOG A (NOLOCK)
                            JOIN [GENERAL].[dbo].IT4_WMS_DOCUMENTO C (NOLOCK) ON A.PEDIDO = TRIM(C.DOCUMENTO)
                            WHERE 

                            StatusFlash != 'Comprovante registrado' 
                            AND StatusFlash != 'Entregue pelo Terceiro'
	                        
                            --TESTE
                            --StatusFlash = 'Comprovante registrado' 
                            --OR StatusFlash = 'Entregue pelo Terceiro'

                            AND DATAENVIO >= DATEADD(DD,-60, GETDATE())

	                        AND C.NB_DATA_COLETA IS NULL 
	                        AND C.NB_DATA_ENTREGA_PREVISAO IS NOT NULL 
	                        AND C.CHAVE_NFE IS NOT NULL 

	                        AND C.NB_TRANSPORTADORA = 18035 
	                        AND C.DATA > GETDATE() - 15";

            try
			{
				using (var conn = _conn.GetIDbConnection())
				{
					return await conn.QueryAsync<FlashCourierRegisterLog>(query, commandTimeout: 360);
				}
			}
			catch (Exception ex)
			{
                throw new Exception(@$"FlashCourier - GetShippedOrders - Erro ao obter pedidos enviados da tabela GENERAL..FlashCourierRegistroLog - {ex.Message}");
            }
        }

        public async Task<FlashCourierParameters> GetAuthenticationUser(string cnpj)
        {
            var query = $@"SELECT DISTINCT LOGIN, SENHA FROM GENERAL.[dbo].PARAMETROS_FLASHCOURIER (NOLOCK) WHERE CNPJ_EMP = '{cnpj}'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<FlashCourierParameters>(query, commandTimeout: 360);
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GetAuthenticationUser - Erro ao obter pedidos enviados da tabela GENERAL..FlashCourierRegistroLog - {ex.Message}");
            }
        }

        public async Task<HAWBRequest> GetAWBRequest(string cnpj)
        {
            var query = $@"SELECT DISTINCT CLIENTEID, CTTID FROM GENERAL.[dbo].PARAMETROS_FLASHCOURIER (NOLOCK) WHERE CNPJ_EMP = '{cnpj}'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<HAWBRequest>(query, commandTimeout: 360);
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GetAuthenticationUser - Erro ao obter pedidos enviados da tabela GENERAL..FlashCourierRegistroLog - {ex.Message}");
            }
        }

        public async Task UpdateCollectionDate(string dtSla, string cardCode)
        {
            string sql = @$"UPDATE GENERAL..[IT4_WMS_DOCUMENTO] SET NB_DATA_COLETA = CONVERT(DATETIME, '{dtSla}', 103) WHERE LTRIM(RTRIM(DOCUMENTO)) = '{cardCode}' AND NB_TRANSPORTADORA = '18035'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    await conn.ExecuteAsync(sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - UpdateCollectionDate - Erro ao atuaizar NB_DATA_COLETA do pedido: {cardCode} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task UpdateDeliveryMadeDate(string occurrence, string cardCode)
        {
            string sql = @$"UPDATE GENERAL..[IT4_WMS_DOCUMENTO] SET NB_DATA_ENTREGA_REALIZADA = CONVERT(DATETIME, '{occurrence}', 103) WHERE LTRIM(RTRIM(DOCUMENTO)) = '{cardCode}' AND NB_TRANSPORTADORA = '18035';
                            UPDATE [GENERAL].[dbo].[FLASHCOURIERREGISTROLOG] SET STATUSECOM = '56' WHERE PEDIDO = '{cardCode}'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    await conn.ExecuteAsync(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - UpdateDeliveryMadeDate - Erro ao atuaizar NB_DATA_ENTREGA_REALIZADA do pedido: {cardCode} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task UpdateLastStatusDate(string occurrence, string eventId, string _event, string cardCode)
        {
            string sql = @$"UPDATE [GENERAL].[dbo].[IT4_WMS_DOCUMENTO] SET NB_DATA_ULTIMO_STATUS = CONVERT(DATETIME, '{occurrence}', 103), NB_DESCRICAO_ULTIMO_STATUS = '{eventId}-{_event}' WHERE LTRIM(RTRIM(DOCUMENTO)) = '{cardCode}' AND NB_TRANSPORTADORA = '18035';
                            UPDATE [GENERAL].[dbo].[FLASHCOURIERREGISTROLOG] SET STATUSFLASH = '{_event}' WHERE PEDIDO = '{cardCode}'";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    await conn.ExecuteAsync(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - UpdateLastStatusDate - Erro ao atuaizar NB_DATA_ULTIMO_STATUS do pedido: {cardCode} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task UpdateRealDeliveryForecastDate(string dtSla, string cardCode)
        {
            string sql = $"UPDATE GENERAL..[IT4_WMS_DOCUMENTO] SET NB_PREVISAO_REAL_ENTREGA = CONVERT(DATETIME, '{dtSla}', 103) WHERE LTRIM(RTRIM(DOCUMENTO)) = '{cardCode}' AND NB_TRANSPORTADORA = '18035';";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    await conn.ExecuteAsync(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - UpdateRealDeliveryForecastDate - Erro ao atuaizar NB_PREVISAO_REAL_ENTREGA do pedido: {cardCode} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }
    }
}

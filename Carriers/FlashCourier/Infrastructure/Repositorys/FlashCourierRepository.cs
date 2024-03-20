namespace BloomersCarriersIntegrations.FlashCourier.Infrastructure.Repositorys
{
    public class FlashCourierRepository : IFlashCourierRepository
    {
        private readonly ISQLServerConnection _conn;

        public FlashCourierRepository(ISQLServerConnection conn) =>
            _conn = conn;

        public async Task GeraLogSucesso(string pedido, string remetenteID, string retorno, string statusFlash, string chaveNFe)
        {
            var sql = $@"INSERT INTO [GENERAL].[dbo].FlashCourierRegistroLog (Pedido, DataEnvio, Retorno, Remetente, StatusFlash, ChaveNFe) 
                         VALUES(@Pedido, GETDATE(), @Retorno, @RemetenteID, @StatusFlash, @ChaveNFe)";
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, new { pedido = pedido, retorno = retorno, remetenteid = remetenteID, statusFlash = statusFlash, chaveNFe = chaveNFe });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GeraLogSucesso - Erro ao inserir registro: {pedido} na tabela GENERAL..FlashCourierRegistroLog - {ex.Message}");
            }
        }

        public Pedido GetPedidoFaturado(string orderNumber)
        {
            try
            {
                string sql = $@"SELECT DISTINCT
	                            TRIM(A.DOCUMENTO) as nr_pedido,
	                            SUM(C.PESO_BRUTO) as peso_bruto,
	                            B.QTDERETORNO as quantidade,

	                            A.NB_CODIGO_CLIENTE as cod_cliente,
	                            A.NB_DOC_CLIENTE as doc_cliente,
	                            A.NB_RAZAO_CLIENTE as razao_cliente,
	                            A.NB_EMAIL_CLIENTE as email_cliente,
	                            A.NB_ENDERECO_CLIENTE as endereco_cliente,
	                            A.NB_NUMERO_RUA_CLIENTE as numero_rua_cliente,
	                            A.NB_COMPLEMENTO_END_CLIENTE as complement_end_cli,
	                            A.NB_BAIRRO_CLIENTE as bairro_cliente,
	                            A.NB_CIDADE as cidade_cliente,
	                            A.NB_ESTADO as uf_cliente,
	                            A.NB_CEP as cep_cliente,
	                            A.NB_FONE_CLIENTE as fone_cliente,
	                            A.NB_INSCRICAO_ESTADUAL_CLIENTE as inscricao_estadual_cliente,
	
	                            A.NB_DOC_REMETENTE as doc_empresa,
	
	                            A.NF_SAIDA as numero_nf,
	                            A.NB_VALOR_PEDIDO AS total_nf,
	                            A.CHAVE_NFE as chave_nfe_nf
                            FROM 
	                            GENERAL..IT4_WMS_DOCUMENTO (NOLOCK) A
                            JOIN 
	                            GENERAL..IT4_WMS_DOCUMENTO_ITEM B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE 
                            JOIN 
	                            BLOOMERS_LINX..LINXPRODUTOS_TRUSTED C (NOLOCK) ON B.CODIGO_BARRA = C.COD_PRODUTO
                            WHERE 
	                            A.NB_TRANSPORTADORA = '18035'
	                            AND A.DOCUMENTO = '{orderNumber}'
	                            AND A.CHAVE_NFE != ''
	                            AND A.[DATA] > '2023-11-01'
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

                using (var conn = _conn.GetDbConnection())
                {
                    return conn.Query<Pedido, Cliente, Empresa, NotaFiscal, Pedido>(sql, (pedido, cliente, empresa, notaFiscal) =>
                    {
                        pedido.cliente = cliente;
                        pedido.empresa = empresa;
                        pedido.notaFiscal = notaFiscal;
                        return pedido;
                    }, splitOn: "cod_cliente, doc_empresa, numero_nf").First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GetPedidoFaturado - Erro ao obter pedido: {orderNumber} da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task<IEnumerable<FlashCourierRegistroLog>> GetPedidosEnviados()
        {
            var query = @"SELECT DISTINCT
                            A.Pedido,
                            A.DataEnvio,
                            A.Retorno,
                            A.StatusEcom,
                            A.Remetente,
                            A.StatusFlash,
                            A.ChaveNFe
                            FROM 
                            [GENERAL].[dbo].FlashCourierRegistroLog A (NOLOCK)
                            JOIN
                            [GENERAL].[dbo].IT4_WMS_DOCUMENTO C (NOLOCK) ON A.PEDIDO = TRIM(C.DOCUMENTO)
                            WHERE 

                            --StatusFlash != 'Comprovante registrado' 
                            --AND StatusFlash != 'Entregue pelo Terceiro'
	                        
                            StatusFlash = 'Comprovante registrado' 
                            OR StatusFlash = 'Entregue pelo Terceiro'

                            AND DataEnvio >= dateadd(DD,-60, getdate())

	                        AND C.NB_DATA_COLETA IS NULL 
	                        AND C.NB_DATA_ENTREGA_PREVISAO IS NOT NULL 
	                        AND C.CHAVE_NFE IS NOT NULL 

	                        AND C.NB_TRANSPORTADORA = 18035 
	                        AND C.DATA > '2023-01-01'";

            try
			{
				using (var conn = _conn.GetDbConnection())
				{
					return await conn.QueryAsync<FlashCourierRegistroLog>(query);
				}
			}
			catch (Exception ex)
			{
                throw new Exception(@$"FlashCourier - GetPedidosEnviados - Erro ao obter pedidos enviados da tabela GENERAL..FlashCourierRegistroLog - {ex.Message}");
            }
        }

        public async Task<IEnumerable<Pedido>> GetPedidosFaturados()
        {
            try
            {
                string sql = @"SELECT DISTINCT
	                            TRIM(A.DOCUMENTO) as nr_pedido,
	                            SUM(C.PESO_BRUTO) as peso_bruto,
	                            B.QTDERETORNO as quantidade,

	                            A.NB_CODIGO_CLIENTE as cod_cliente,
	                            A.NB_DOC_CLIENTE as doc_cliente,
	                            A.NB_RAZAO_CLIENTE as razao_cliente,
	                            A.NB_EMAIL_CLIENTE as email_cliente,
	                            A.NB_ENDERECO_CLIENTE as endereco_cliente,
	                            A.NB_NUMERO_RUA_CLIENTE as numero_rua_cliente,
	                            A.NB_COMPLEMENTO_END_CLIENTE as complement_end_cli,
	                            A.NB_BAIRRO_CLIENTE as bairro_cliente,
	                            A.NB_CIDADE as cidade_cliente,
	                            A.NB_ESTADO as uf_cliente,
	                            A.NB_CEP as cep_cliente,
	                            A.NB_FONE_CLIENTE as fone_cliente,
	                            A.NB_INSCRICAO_ESTADUAL_CLIENTE as inscricao_estadual_cliente,
	
	                            A.NB_DOC_REMETENTE as doc_empresa,
	
	                            A.NF_SAIDA as numero_nf,
	                            A.NB_VALOR_PEDIDO AS total_nf,
	                            A.CHAVE_NFE as chave_nfe_nf
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
	                            --AND A.DOCUMENTO IN ('')
	                            AND D.PEDIDO IS NULL
	                            AND A.CHAVE_NFE != ''
	                            AND A.[DATA] > '2023-11-01'
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

                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryAsync<Pedido, Cliente, Empresa, NotaFiscal, Pedido>(sql, (pedido, cliente, empresa, notaFiscal) =>
                    {
                        pedido.cliente = cliente;
                        pedido.empresa = empresa;
                        pedido.notaFiscal = notaFiscal;
                        return pedido;
                    }, splitOn: "cod_cliente, doc_empresa, numero_nf");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GetPedidosFaturados - Erro ao obter pedidos da tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task Update_NB_DATA_COLETA(string dtSla, string codigoCartao)
        {
            string sql = @$"UPDATE GENERAL..[IT4_WMS_DOCUMENTO] SET NB_DATA_COLETA = convert(datetime, '{dtSla}', 103) WHERE LTRIM(RTRIM(Documento)) = '{codigoCartao}' AND NB_TRANSPORTADORA = '18035'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - Update_NB_DATA_COLETA - Erro ao atuaizar NB_DATA_COLETA do pedido: {codigoCartao} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task Update_NB_DATA_ENTREGA_REALIZADA(string ocorrencia, string codigoCartao)
        {
            string sql = @$"UPDATE GENERAL..[IT4_WMS_DOCUMENTO] SET NB_DATA_ENTREGA_REALIZADA = convert(datetime, '{ocorrencia}', 103) WHERE LTRIM(RTRIM(Documento)) = '{codigoCartao}' AND NB_TRANSPORTADORA = '18035';
                            UPDATE [GENERAL].[dbo].[FlashCourierRegistroLog] SET StatusEcom = '56' WHERE Pedido = '{codigoCartao}'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - Update_NB_DATA_ENTREGA_REALIZADA - Erro ao atuaizar NB_DATA_ENTREGA_REALIZADA do pedido: {codigoCartao} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task Update_NB_DATA_ULTIMO_STATUS(string ocorrencia, string eventoId, string evento, string codigoCartao)
        {
            string sql = @$"UPDATE [GENERAL].[dbo].[IT4_WMS_DOCUMENTO] SET NB_DATA_ULTIMO_STATUS = convert(datetime, '{ocorrencia}', 103), NB_DESCRICAO_ULTIMO_STATUS = '{eventoId}-{evento}' WHERE LTRIM(RTRIM(Documento)) = '{codigoCartao}' AND NB_TRANSPORTADORA = '18035';
                            UPDATE [GENERAL].[dbo].[FlashCourierRegistroLog] SET StatusFlash = '{evento}' WHERE Pedido = '{codigoCartao}'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - Update_NB_DATA_ULTIMO_STATUS - Erro ao atuaizar NB_DATA_ULTIMO_STATUS do pedido: {codigoCartao} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task Update_NB_PREVISAO_REAL_ENTREGA(string dtSla, string codigoCartao)
        {
            string sql = $"UPDATE GENERAL..[IT4_WMS_DOCUMENTO] SET NB_PREVISAO_REAL_ENTREGA = convert(datetime, '{dtSla}', 103) WHERE LTRIM(RTRIM(Documento)) = '{codigoCartao}' AND NB_TRANSPORTADORA = '18035';";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - Update_NB_PREVISAO_REAL_ENTREGA - Erro ao atuaizar NB_PREVISAO_REAL_ENTREGA do pedido: {codigoCartao} na tabela GENERAL..IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }
    }
}

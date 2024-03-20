using Dapper;
using System.Data;
using System.Data.SqlClient;
using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using Microvix.Models;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys
{
    public class LinxPedidosVendaRepository<T1> : ILinxPedidosVendaRepository<T1> where T1 : LinxPedidosVenda, new()
    {
        private readonly ISQLServerConnection _conn;

        public LinxPedidosVendaRepository(ISQLServerConnection conn) =>
            _conn = conn;

        public void BulkInsertIntoTableRaw(List<T1> registros, string? tableName, string? db)
        {
            try
            {
                var table = new DataTable();
                var properties = registros[0].GetType().GetProperties();

                for (int i = 0; i < properties.Count(); i++)
                {
                    table.Columns.Add($"{properties[i].Name}");
                }

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cnpj_emp, registros[i].cod_pedido, registros[i].data_lancamento, registros[i].hora_lancamento,
                                   registros[i].transacao, registros[i].usuario, registros[i].codigo_cliente, registros[i].cod_produto, registros[i].quantidade, registros[i].valor_unitario,
                                   registros[i].cod_vendedor, registros[i].valor_frete, registros[i].valor_total, registros[i].desconto_item, registros[i].cod_plano_pagamento, registros[i].plano_pagamento,
                                   registros[i].obs, registros[i].aprovado, registros[i].cancelado, registros[i].data_aprovacao, registros[i].data_alteracao, registros[i].tipo_frete,
                                   registros[i].natureza_operacao, registros[i].tabela_preco, registros[i].nome_tabela_preco, registros[i].previsao_entrega, registros[i].realizado_por,
                                   registros[i].pontuacao_ser, registros[i].venda_externa, registros[i].nf_gerada, registros[i].status, registros[i].numero_projeto_officina, registros[i].cod_natureza_operacao,
                                   registros[i].margem_contribuicao, registros[i].doc_origem, registros[i].posicao_item, registros[i].orcamento_origem, registros[i].transacao_origem, registros[i].timestamp,
                                   registros[i].desconto, registros[i].transacao_ws, registros[i].empresa, registros[i].transportador, registros[i].deposito);
                }

                using (var conn = _conn.GetDbConnection())
                {
                    using var bulkCopy = new SqlBulkCopy((SqlConnection)conn);
                    bulkCopy.DestinationTableName = $"{db}.[dbo].{tableName}_raw";
                    bulkCopy.BatchSize = table.Rows.Count;
                    bulkCopy.BulkCopyTimeout = 5 * 60;
                    bulkCopy.WriteToServer(table);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPedidosVenda - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
            }
        }

        public async Task CallDbProcMerge(string? procName, string? tableName, string? db)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync($"{db}..{procName}", commandTimeout: 180, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPedidosVenda - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
            }
        }

        public void CallDbProcMergeSync(string? procName, string? tableName, string? db)
        {
            try
            {
                using (var conn = _conn.GetSqlDbConnection())
                {
                    using (var command = new SqlCommand($"{db}..{procName}", conn) { CommandType = CommandType.StoredProcedure })
                    {
                        command.CommandTimeout = 120;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPedidosVenda - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
            }
        }

        public async Task<string> GetParameters(string tableName, string parameterCol)
        {
            string sql = $@"SELECT {parameterCol} FROM [BLOOMERS_LINX].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryFirstAsync<string>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPedidosVenda - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public string GetParametersSync(string tableName, string parameterCol)
        {
            string sql = $@"SELECT {parameterCol} FROM [BLOOMERS_LINX].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return conn.QueryFirst<string>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPedidosVenda - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task InsereRegistroIndividual(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [cod_pedido], [data_lancamento], [hora_lancamento], [transacao], [usuario], [codigo_cliente], [cod_produto],
                             [quantidade], [valor_unitario], [cod_vendedor], [valor_frete], [valor_total], [desconto_item], [cod_plano_pagamento], [plano_pagamento], [obs],
                             [aprovado], [cancelado], [data_aprovacao], [data_alteracao], [tipo_frete], [natureza_operacao], [tabela_preco], [nome_tabela_preco], [previsao_entrega],
                             [realizado_por], [pontuacao_ser], [venda_externa], [nf_gerada], [status], [numero_projeto_officina], [cod_natureza_operacao], [margem_contribuicao],
                             [doc_origem], [posicao_item], [orcamento_origem], [transacao_origem], [timestamp], [desconto], [transacao_ws], [empresa], [transportador], [deposito]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @cod_pedido, @data_lancamento, @hora_lancamento, @transacao, @usuario, @codigo_cliente, @cod_produto,
                             @quantidade, @valor_unitario, @cod_vendedor, @valor_frete, @valor_total, @desconto_item, @cod_plano_pagamento, @plano_pagamento, @obs,
                             @aprovado, @cancelado, @data_aprovacao, @data_alteracao, @tipo_frete, @natureza_operacao, @tabela_preco, @nome_tabela_preco, @previsao_entrega,
                             @realizado_por, @pontuacao_ser, @venda_externa, @nf_gerada, @status, @numero_projeto_officina, @cod_natureza_operacao, @margem_contribuicao,
                             @doc_origem, @posicao_item, @orcamento_origem, @transacao_origem, @timestamp, @desconto, @transacao_ws, @empresa, @transportador, @deposito
                            )";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPedidosVenda - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [cod_pedido], [data_lancamento], [hora_lancamento], [transacao], [usuario], [codigo_cliente], [cod_produto],
                             [quantidade], [valor_unitario], [cod_vendedor], [valor_frete], [valor_total], [desconto_item], [cod_plano_pagamento], [plano_pagamento], [obs],
                             [aprovado], [cancelado], [data_aprovacao], [data_alteracao], [tipo_frete], [natureza_operacao], [tabela_preco], [nome_tabela_preco], [previsao_entrega],
                             [realizado_por], [pontuacao_ser], [venda_externa], [nf_gerada], [status], [numero_projeto_officina], [cod_natureza_operacao], [margem_contribuicao],
                             [doc_origem], [posicao_item], [orcamento_origem], [transacao_origem], [timestamp], [desconto], [transacao_ws], [empresa], [transportador], [deposito]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @cod_pedido, @data_lancamento, @hora_lancamento, @transacao, @usuario, @codigo_cliente, @cod_produto,
                             @quantidade, @valor_unitario, @cod_vendedor, @valor_frete, @valor_total, @desconto_item, @cod_plano_pagamento, @plano_pagamento, @obs,
                             @aprovado, @cancelado, @data_aprovacao, @data_alteracao, @tipo_frete, @natureza_operacao, @tabela_preco, @nome_tabela_preco, @previsao_entrega,
                             @realizado_por, @pontuacao_ser, @venda_externa, @nf_gerada, @status, @numero_projeto_officina, @cod_natureza_operacao, @margem_contribuicao,
                             @doc_origem, @posicao_item, @orcamento_origem, @transacao_origem, @timestamp, @desconto, @transacao_ws, @empresa, @transportador, @deposito
                            )";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    conn.Execute(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPedidosVenda - InsereRegistroIndividualSync - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<IEnumerable<Empresa>> GetEmpresas()
        {
            string sql = $@"SELECT empresa as numero_erp_empresa, nome_emp as nome_empresa, cnpj_emp as doc_empresa FROM BLOOMERS_LINX..LinxLojas_trusted WHERE nome_emp LIKE '%MISHA%' or nome_emp LIKE '%OPEN%'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryAsync<Empresa>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPedidosVenda - GetEmpresas - Erro ao obter as empresas da tabela LinxLojas_trusted, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public IEnumerable<Empresa> GetEmpresasSync()
        {
            string sql = $@"SELECT empresa as numero_erp_empresa, nome_emp as nome_empresa, cnpj_emp as doc_empresa FROM BLOOMERS_LINX..LinxLojas_trusted WHERE nome_emp LIKE '%MISHA%' or nome_emp LIKE '%OPEN%'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return conn.Query<Empresa>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPedidosVenda - GetEmpresasSync - Erro ao obter as empresas da tabela LinxLojas_trusted, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<List<T1>> GetRegistersExists(List<T1> registros, string? tableName, string? db)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj_emp}'";
                else
                    identificadores += $"'{registros[i].cnpj_emp}', ";
            }
            string query = $"SELECT cnpj_emp, cod_produto, cod_pedido, timestamp FROM {db}.[dbo].{tableName} WHERE cod_pedido IN ({identificadores})";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    var result = await conn.QueryAsync<T1>(query, commandTimeout: 120);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

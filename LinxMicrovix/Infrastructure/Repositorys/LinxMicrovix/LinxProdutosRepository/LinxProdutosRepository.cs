using Dapper;
using System.Data;
using System.Data.SqlClient;
using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys
{
    public class LinxProdutosRepository<T1> : ILinxProdutosRepository<T1> where T1 : LinxProdutos, new()
    {
        private readonly ISQLServerConnection _conn;

        public LinxProdutosRepository(ISQLServerConnection conn) =>
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
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cod_produto, registros[i].cod_barra, registros[i].nome, registros[i].ncm, registros[i].cest, registros[i].referencia, registros[i].cod_auxiliar, registros[i].unidade, registros[i].desc_cor, registros[i].desc_tamanho,
                                   registros[i].desc_setor, registros[i].desc_linha, registros[i].desc_marca, registros[i].desc_colecao, registros[i].dt_update, registros[i].cod_fornecedor, registros[i].desativado, registros[i].desc_espessura, registros[i].id_espessura, registros[i].desc_classificacao,
                                   registros[i].id_classificacao, registros[i].origem_mercadoria, registros[i].peso_liquido, registros[i].peso_bruto, registros[i].id_cor, registros[i].id_tamanho, registros[i].id_setor, registros[i].id_linha, registros[i].id_marca, registros[i].id_colecao, registros[i].dt_inclusao,
                                   registros[i].timestamp, registros[i].fator_conversao, registros[i].codigo_integracao_ws, registros[i].codigo_integracao_reshop, registros[i].id_produtos_opticos_tipo, registros[i].id_sped_tipo_item, registros[i].componente, registros[i].altura_para_frete,
                                   registros[i].largura_para_frete, registros[i].comprimento_para_frete, registros[i].loja_virtual, registros[i].codigoproduto_io, registros[i].cod_comprador, registros[i].altura, registros[i].largura, registros[i].comprimento, registros[i].codigo_integracao_oms);
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
                throw new Exception($"LinxProdutos - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
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
                throw new Exception($"LinxProdutos - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxProdutos - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxProdutos - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
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
                throw new Exception($"LinxProdutos - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public Task<List<T1>> GetRegistersExists(List<T1> registros, string? tableName, string? db)
        {
            throw new NotImplementedException();
        }

        public async Task InsereRegistroIndividual(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_produto], [cod_barra], [nome], [ncm], [cest], [referencia], [cod_auxiliar], [unidade], [desc_cor], [desc_tamanho], 
                             [desc_setor], [desc_linha], [desc_marca], [desc_colecao], [dt_update], [cod_fornecedor], [desativado], [desc_espessura], [id_espessura], [desc_classificacao],
                             [id_classificacao], [origem_mercadoria], [peso_liquido], [peso_bruto], [id_cor], [id_tamanho], [id_setor], [id_linha], [id_marca], [id_colecao], [dt_inclusao],
                             [timestamp], [fator_conversao], [codigo_integracao_ws], [codigo_integracao_reshop], [id_produtos_opticos_tipo], [id_sped_tipo_item], [componente], [altura_para_frete],
                             [largura_para_frete], [comprimento_para_frete], [loja_virtual], [codigoproduto_io], [cod_comprador], [altura], [largura], [comprimento], [codigo_integracao_oms]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cod_produto, @cod_barra, @nome, @ncm, @cest, @referencia, @cod_auxiliar, @unidade, @desc_cor, @desc_tamanho, 
                             @desc_setor, @desc_linha, @desc_marca, @desc_colecao, @dt_update, @cod_fornecedor, @desativado, @desc_espessura, @id_espessura, @desc_classificacao,
                             @id_classificacao, @origem_mercadoria, @peso_liquido, @peso_bruto, @id_cor, @id_tamanho, @id_setor, @id_linha, @id_marca, @id_colecao, @dt_inclusao,
                             @timestamp, @fator_conversao, @codigo_integracao_ws, @codigo_integracao_reshop, @id_produtos_opticos_tipo, @id_sped_tipo_item, @componente, @altura_para_frete,
                             @largura_para_frete, @comprimento_para_frete, @loja_virtual, @codigoproduto_io, @cod_comprador, @altura, @largura, @comprimento, @codigo_integracao_oms
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
                throw new Exception($"LinxProdutos - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_produto], [cod_barra], [nome], [ncm], [cest], [referencia], [cod_auxiliar], [unidade], [desc_cor], [desc_tamanho], 
                             [desc_setor], [desc_linha], [desc_marca], [desc_colecao], [dt_update], [cod_fornecedor], [desativado], [desc_espessura], [id_espessura], [desc_classificacao],
                             [id_classificacao], [origem_mercadoria], [peso_liquido], [peso_bruto], [id_cor], [id_tamanho], [id_setor], [id_linha], [id_marca], [id_colecao], [dt_inclusao],
                             [timestamp], [fator_conversao], [codigo_integracao_ws], [codigo_integracao_reshop], [id_produtos_opticos_tipo], [id_sped_tipo_item], [componente], [altura_para_frete],
                             [largura_para_frete], [comprimento_para_frete], [loja_virtual], [codigoproduto_io], [cod_comprador], [altura], [largura], [comprimento], [codigo_integracao_oms]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cod_produto, @cod_barra, @nome, @ncm, @cest, @referencia, @cod_auxiliar, @unidade, @desc_cor, @desc_tamanho, 
                             @desc_setor, @desc_linha, @desc_marca, @desc_colecao, @dt_update, @cod_fornecedor, @desativado, @desc_espessura, @id_espessura, @desc_classificacao,
                             @id_classificacao, @origem_mercadoria, @peso_liquido, @peso_bruto, @id_cor, @id_tamanho, @id_setor, @id_linha, @id_marca, @id_colecao, @dt_inclusao,
                             @timestamp, @fator_conversao, @codigo_integracao_ws, @codigo_integracao_reshop, @id_produtos_opticos_tipo, @id_sped_tipo_item, @componente, @altura_para_frete,
                             @largura_para_frete, @comprimento_para_frete, @loja_virtual, @codigoproduto_io, @cod_comprador, @altura, @largura, @comprimento, @codigo_integracao_oms
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
                throw new Exception($"LinxProdutos - InsereRegistroIndividualSync - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }
    }
}

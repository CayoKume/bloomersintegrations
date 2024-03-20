using Dapper;
using System.Data;
using System.Data.SqlClient;
using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys
{
    public class LinxClientesFornecRepository<T1> : ILinxClientesFornecRepository<T1> where T1 : LinxClientesFornec, new()
    {
        private readonly ISQLServerConnection _conn;

        public LinxClientesFornecRepository(ISQLServerConnection conn) =>
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
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cod_cliente, registros[i].razao_cliente, registros[i].nome_cliente, registros[i].doc_cliente, registros[i].tipo_cliente, registros[i].endereco_cliente,
                        registros[i].numero_rua_cliente, registros[i].complement_end_cli, registros[i].bairro_cliente, registros[i].cep_cliente, registros[i].cidade_cliente, registros[i].uf_cliente, registros[i].pais,
                        registros[i].fone_cliente, registros[i].email_cliente, registros[i].sexo, registros[i].data_cadastro, registros[i].data_nascimento, registros[i].cel_cliente, registros[i].ativo, registros[i].dt_update,
                        registros[i].inscricao_estadual, registros[i].incricao_municipal, registros[i].identidade_cliente, registros[i].cartao_fidelidade, registros[i].cod_ibge_municipio, registros[i].classe_cliente, registros[i].matricula_conveniado, registros[i].tipo_cadastro, registros[i].empresa_cadastro,
                        registros[i].id_estado_civil, registros[i].fax_cliente, registros[i].site_cliente, registros[i].timestamp, registros[i].cliente_anonimo, registros[i].limite_compras, registros[i].codigo_ws, registros[i].limite_credito_compra, registros[i].id_classe_fiscal, registros[i].obs, registros[i].mae, registros[i].cliente_contribuinte);
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
                throw new Exception($"LinxClientesFornec - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
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
                throw new Exception($"LinxClientesFornec - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxClientesFornec - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxClientesFornec - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
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
                throw new Exception($"LinxClientesFornec - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<List<T1>> GetRegistersExists(List<T1> registros, string? tableName, string? db)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].doc_cliente}'";
                else
                    identificadores += $"'{registros[i].doc_cliente}', ";
            }
            string query = $"SELECT DOC_CLIENTE, TIMESTAMP FROM {db}.[dbo].{tableName} WHERE DOC_CLIENTE IN ({identificadores})";

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

        public async Task InsereRegistroIndividual(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_cliente], [razao_cliente], [nome_cliente], [doc_cliente], [tipo_cliente], [endereco_cliente],
                             [numero_rua_cliente], [complement_end_cli], [bairro_cliente], [cep_cliente], [cidade_cliente], [uf_cliente], [pais],
                             [fone_cliente], [email_cliente], [sexo], [data_cadastro], [data_nascimento], [cel_cliente], [ativo], [dt_update],
                             [inscricao_estadual], [incricao_municipal], [identidade_cliente], [cartao_fidelidade], [cod_ibge_municipio], [classe_cliente], 
                             [matricula_conveniado], [tipo_cadastro], [empresa_cadastro], [id_estado_civil], [fax_cliente], [site_cliente], [timestamp], 
                             [cliente_anonimo], [limite_compras], [codigo_ws], [limite_credito_compra], [id_classe_fiscal], [obs], [mae], [cliente_contribuinte]) 
                            Values 
                            (@lastupdateon, @portal, @cod_cliente, @razao_cliente, @nome_cliente, @doc_cliente, @tipo_cliente, @endereco_cliente,
                             @numero_rua_cliente, @complement_end_cli, @bairro_cliente, @cep_cliente, @cidade_cliente, @uf_cliente, @pais,
                             @fone_cliente, @email_cliente, @sexo, @data_cadastro, @data_nascimento, @cel_cliente, @ativo, @dt_update,
                             @inscricao_estadual, @incricao_municipal, @identidade_cliente, @cartao_fidelidade, @cod_ibge_municipio, @classe_cliente, 
                             @matricula_conveniado, @tipo_cadastro, @empresa_cadastro, @id_estado_civil, @fax_cliente, @site_cliente, @timestamp, 
                             @cliente_anonimo, @limite_compras, @codigo_ws, @limite_credito_compra, @id_classe_fiscal, @obs, @mae, @cliente_contribuinte)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxClientesFornec - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_cliente], [razao_cliente], [nome_cliente], [doc_cliente], [tipo_cliente], [endereco_cliente],
                             [numero_rua_cliente], [complement_end_cli], [bairro_cliente], [cep_cliente], [cidade_cliente], [uf_cliente], [pais],
                             [fone_cliente], [email_cliente], [sexo], [data_cadastro], [data_nascimento], [cel_cliente], [ativo], [dt_update],
                             [inscricao_estadual], [incricao_municipal], [identidade_cliente], [cartao_fidelidade], [cod_ibge_municipio], [classe_cliente], 
                             [matricula_conveniado], [tipo_cadastro], [empresa_cadastro], [id_estado_civil], [fax_cliente], [site_cliente], [timestamp], 
                             [cliente_anonimo], [limite_compras], [codigo_ws], [limite_credito_compra], [id_classe_fiscal], [obs], [mae], [cliente_contribuinte]) 
                            Values 
                            (@lastupdateon, @portal, @cod_cliente, @razao_cliente, @nome_cliente, @doc_cliente, @tipo_cliente, @endereco_cliente,
                             @numero_rua_cliente, @complement_end_cli, @bairro_cliente, @cep_cliente, @cidade_cliente, @uf_cliente, @pais,
                             @fone_cliente, @email_cliente, @sexo, @data_cadastro, @data_nascimento, @cel_cliente, @ativo, @dt_update,
                             @inscricao_estadual, @incricao_municipal, @identidade_cliente, @cartao_fidelidade, @cod_ibge_municipio, @classe_cliente, 
                             @matricula_conveniado, @tipo_cadastro, @empresa_cadastro, @id_estado_civil, @fax_cliente, @site_cliente, @timestamp, 
                             @cliente_anonimo, @limite_compras, @codigo_ws, @limite_credito_compra, @id_classe_fiscal, @obs, @mae, @cliente_contribuinte)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    conn.Execute(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxClientesFornec - InsereRegistroIndividualSync - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }
    }
}

using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys
{
    public class LinxLojasRepository<T1> : ILinxLojasRepository<T1> where T1 : LinxLojas, new()
    {
        private readonly ISQLServerConnection _conn;

        public LinxLojasRepository(ISQLServerConnection conn) =>
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
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].empresa, registros[i].nome_emp, registros[i].razao_emp, registros[i].cnpj_emp, registros[i].inscricao_emp, registros[i].endereco_emp,
                        registros[i].num_emp, registros[i].complement_emp, registros[i].bairro_emp, registros[i].cep_emp, registros[i].cidade_emp, registros[i].estado_emp, registros[i].fone_emp,
                        registros[i].email_emp, registros[i].cod_ibge_municipio, registros[i].data_criacao_emp, registros[i].data_criacao_portal, registros[i].sistema_tributacao, registros[i].regime_tributario, registros[i].area_empresa, registros[i].timestamp,
                        registros[i].sigla_empresa, registros[i].id_classe_fiscal, registros[i].centro_distribuicao, registros[i].cnae_emp, registros[i].cod_cliente_linx);
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
                throw new Exception($"LinxLojas - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
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
                throw new Exception($"LinxLojas - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxLojas - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxLojas - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
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
                throw new Exception($"LinxLojas - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
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
            string query = $"SELECT cnpj_emp, TIMESTAMP FROM BLOOMERS_LINX..LinxLojas_trusted WHERE cnpj_emp IN ({identificadores})";

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
                            ([lastupdateon], [portal], [empresa], [nome_emp], [razao_emp], [cnpj_emp], [inscricao_emp], [endereco_emp],
                             [num_emp], [complement_emp], [bairro_emp], [cep_emp], [cidade_emp], [estado_emp], [fone_emp],
                             [email_emp], [cod_ibge_municipio], [data_criacao_emp], [data_criacao_portal], [sistema_tributacao], [regime_tributario], [area_empresa], [timestamp],
                             [sigla_empresa], [id_classe_fiscal], [centro_distribuicao], [inscricao_municipal_emp], [cnae_emp], [cod_cliente_linx]) 
                            Values 
                            (@lastupdateon, @portal, @empresa, @nome_emp, @razao_emp, @cnpj_emp, @inscricao_emp, @endereco_emp,
                             @num_emp, @complement_emp, @bairro_emp, @cep_emp, @cidade_emp, @estado_emp, @fone_emp,
                             @email_emp, @cod_ibge_municipio, @data_criacao_emp, @data_criacao_portal, @sistema_tributacao, @regime_tributario, @area_empresa, @timestamp,
                             @sigla_empresa, @id_classe_fiscal, @centro_distribuicao, @inscricao_municipal_emp, @cnae_emp, @cod_cliente_linx)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxLojas - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [portal], [empresa], [nome_emp], [razao_emp], [cnpj_emp], [inscricao_emp], [endereco_emp],
                             [num_emp], [complement_emp], [bairro_emp], [cep_emp], [cidade_emp], [estado_emp], [fone_emp],
                             [email_emp], [cod_ibge_municipio], [data_criacao_emp], [data_criacao_portal], [sistema_tributacao], [regime_tributario], [area_empresa], [timestamp],
                             [sigla_empresa], [id_classe_fiscal], [centro_distribuicao], [inscricao_municipal_emp], [cnae_emp], [cod_cliente_linx]) 
                            Values 
                            (@lastupdateon, @portal, @empresa, @nome_emp, @razao_emp, @cnpj_emp, @inscricao_emp, @endereco_emp,
                             @num_emp, @complement_emp, @bairro_emp, @cep_emp, @cidade_emp, @estado_emp, @fone_emp,
                             @email_emp, @cod_ibge_municipio, @data_criacao_emp, @data_criacao_portal, @sistema_tributacao, @regime_tributario, @area_empresa, @timestamp,
                             @sigla_empresa, @id_classe_fiscal, @centro_distribuicao, @inscricao_municipal_emp, @cnae_emp, @cod_cliente_linx)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    conn.Execute(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxLojas - InsereRegistroIndividualSync - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }
    }
}

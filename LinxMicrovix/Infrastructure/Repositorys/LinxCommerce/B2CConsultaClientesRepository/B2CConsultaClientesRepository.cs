using Dapper;
using System.Data;
using System.Data.SqlClient;
using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Models.Ecommerce;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Repositorys.Interfaces;

namespace BloomersMicrovixIntegrations.Repositorys.Ecommerce
{
    public class B2CConsultaClientesRepository<T1> : IB2CConsultaClientesRepository<T1> where T1 : B2CConsultaClientes, new()
    {
        private readonly ISQLServerConnection _conn;

        public B2CConsultaClientesRepository(ISQLServerConnection conn) =>
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
                    table.Rows.Add(registros[i].lastupdateon, registros[i].cod_cliente_b2c, registros[i].cod_cliente_erp, registros[i].doc_cliente, registros[i].nm_cliente, registros[i].nm_mae, registros[i].nm_pai, registros[i].nm_conjuge,
                        registros[i].dt_cadastro.Value.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].dt_nasc_cliente.Value.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].end_cliente, registros[i].complemento_end_cliente, registros[i].nr_rua_cliente, registros[i].bairro_cliente, registros[i].cep_cliente,
                        registros[i].cidade_cliente, registros[i].uf_cliente, registros[i].fone_cliente, registros[i].fone_comercial, registros[i].cel_cliente, registros[i].email_cliente, registros[i].rg_cliente, registros[i].rg_orgao_emissor,
                        registros[i].estado_civil_cliente, registros[i].empresa_cliente, registros[i].cargo_cliente, registros[i].sexo_cliente, registros[i].dt_update.Value.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].ativo, registros[i].receber_email, registros[i].dt_expedicao_rg.Value.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].naturalidade,
                        registros[i].tempo_residencia, registros[i].renda, registros[i].numero_compl_rua_cliente, registros[i].timestamp, registros[i].tipo_pessoa, registros[i].portal);
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
                throw new Exception($"B2CConsultaClientes - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
            }
        }

        public async Task CallDbProcMerge(string? procName, string? tableName, string? database)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync($"{database}..{procName}", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaClientes - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"B2CConsultaClientes - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
            }
        }

        public async Task<string> GetLastTimestampClientesERP()
        {
            string sql = $@"SELECT MIN(TIMESTAMP) 
                            FROM [BLOOMERS_LINX].[dbo].[LINXCLIENTESFORNEC_TRUSTED] A (nolock) 
                            WHERE 
                            --DOC_CLIENTE IN ()
                            DATA_CADASTRO > '{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}T00:00:00' 
                            AND DATA_CADASTRO < '{DateTime.Today.ToString("yyyy-MM-dd")}T23:59:59'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryFirstAsync<string>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaClientes - GetLastTimestampClientesERP - Erro ao obter o timestamp (MIN) dos clientes da tabela LINXCLIENTESFORNEC_TRUSTED, atraves do sql {sql} - {ex.Message}");
            }
        }

        public string GetLastTimestampClientesERPSync()
        {
            string sql = $@"SELECT MIN(TIMESTAMP) 
                            FROM [BLOOMERS_LINX].[dbo].[LINXCLIENTESFORNEC_TRUSTED] A (nolock) 
                            WHERE 
                            --DOC_CLIENTE IN ()
                            DATA_CADASTRO > '{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}T00:00:00' 
                            AND DATA_CADASTRO < '{DateTime.Today.ToString("yyyy-MM-dd")}T23:59:59'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return conn.QueryFirst<string>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaClientes - GetLastTimestampClientesERPSync - Erro ao obter o timestamp (MIN) dos clientes da tabela LINXCLIENTESFORNEC_TRUSTED, atraves do sql {sql} - {ex.Message}");
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
            string query = $"SELECT DOC_CLIENTE, TIMESTAMP FROM BLOOMERS_LINX..B2CCONSULTACLIENTES_TRUSTED WHERE DOC_CLIENTE IN ({identificadores})";
            
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
                throw new Exception($"B2CConsultaClientes - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atrves do sql: {sql} - {ex.Message}");
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
                throw new Exception($"B2CConsultaClientes - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atrves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task InsereRegistroIndividual(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [cod_cliente_b2c], [cod_cliente_erp], [doc_cliente], [nm_cliente], [nm_mae], [nm_pai], [nm_conjuge], [dt_cadastro], [dt_nasc_cliente], [end_cliente],[complemento_end_cliente], 
                             [nr_rua_cliente], [bairro_cliente], [cep_cliente], [cidade_cliente], [uf_cliente], [fone_cliente], [fone_comercial], [cel_cliente],[email_cliente], [rg_cliente], [rg_orgao_emissor], [estado_civil_cliente], 
                             [empresa_cliente], [cargo_cliente], [sexo_cliente], [dt_update], [ativo], [receber_email],[dt_expedicao_rg], [naturalidade], [tempo_residencia], [renda],[numero_compl_rua_cliente], [timestamp], 
                             [tipo_pessoa], [portal]) 
                            Values 
                            (@lastupdateon, @cod_cliente_b2c, @cod_cliente_erp, @doc_cliente, @nm_cliente, @nm_mae, @nm_pai, @nm_conjuge, @dt_cadastro, @dt_nasc_cliente, @end_cliente,
                             @complemento_end_cliente, @nr_rua_cliente, @bairro_cliente, @cep_cliente, @cidade_cliente, @uf_cliente, @fone_cliente, @fone_comercial, @cel_cliente,
                             @email_cliente, @rg_cliente, @rg_orgao_emissor, @estado_civil_cliente, @empresa_cliente, @cargo_cliente, @sexo_cliente, @dt_update, @ativo, @receber_email,
                             @dt_expedicao_rg, @naturalidade, @tempo_residencia, @renda,@numero_compl_rua_cliente, @timestamp, @tipo_pessoa, @portal)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaClientes - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atrves do sql {sql} - {ex.Message}");
            }
        }

        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [cod_cliente_b2c], [cod_cliente_erp], [doc_cliente], [nm_cliente], [nm_mae], [nm_pai], [nm_conjuge], [dt_cadastro], [dt_nasc_cliente], [end_cliente],[complemento_end_cliente], 
                             [nr_rua_cliente], [bairro_cliente], [cep_cliente], [cidade_cliente], [uf_cliente], [fone_cliente], [fone_comercial], [cel_cliente],[email_cliente], [rg_cliente], [rg_orgao_emissor], [estado_civil_cliente], 
                             [empresa_cliente], [cargo_cliente], [sexo_cliente], [dt_update], [ativo], [receber_email],[dt_expedicao_rg], [naturalidade], [tempo_residencia], [renda],[numero_compl_rua_cliente], [timestamp], 
                             [tipo_pessoa], [portal]) 
                            Values 
                            (@lastupdateon, @cod_cliente_b2c, @cod_cliente_erp, @doc_cliente, @nm_cliente, @nm_mae, @nm_pai, @nm_conjuge, @dt_cadastro, @dt_nasc_cliente, @end_cliente,
                             @complemento_end_cliente, @nr_rua_cliente, @bairro_cliente, @cep_cliente, @cidade_cliente, @uf_cliente, @fone_cliente, @fone_comercial, @cel_cliente,
                             @email_cliente, @rg_cliente, @rg_orgao_emissor, @estado_civil_cliente, @empresa_cliente, @cargo_cliente, @sexo_cliente, @dt_update, @ativo, @receber_email,
                             @dt_expedicao_rg, @naturalidade, @tempo_residencia, @renda,@numero_compl_rua_cliente, @timestamp, @tipo_pessoa, @portal)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    conn.Execute(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaClientes - InsereRegistroIndividualSync - Erro ao inserir registro na tabela {tableName}, atrves do sql {sql} - {ex.Message}");
            }
        }
    }
}

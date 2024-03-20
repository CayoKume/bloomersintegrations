using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys
{
    public class LinxPlanosRepository<T1> : ILinxPlanosRepository<T1> where T1 : LinxPlanos, new()
    {
        private readonly ISQLServerConnection _conn;

        public LinxPlanosRepository(ISQLServerConnection conn) =>
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
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].plano, registros[i].desc_plano, registros[i].qtde_parcelas, registros[i].prazo_entre_parcelas, registros[i].tipo_plano, registros[i].indice_plano,
                                   registros[i].cod_forma_pgto, registros[i].forma_pgto, registros[i].conta_central, registros[i].tipo_transacao, registros[i].taxa_financeira, registros[i].dt_upd, registros[i].desativado, registros[i].usa_tef, 
                                   registros[i].timestamp);
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
                throw new Exception($"LinxPlanos - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
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
                throw new Exception($"LinxPlanos - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxPlanos - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxPlanos - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
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
                throw new Exception($"LinxPlanos - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<List<T1>> GetRegistersExists(List<T1> registros, string? tableName, string? db)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].plano}'";
                else
                    identificadores += $"'{registros[i].plano}', ";
            }
            string query = $"SELECT plano, timestamp FROM {db}.[dbo].{tableName} WHERE plano IN ({identificadores})";

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
                            ([lastupdateon], [portal], [plano], [desc_plano], [qtde_parcelas], [prazo_entre_parcelas], [tipo_plano], [indice_plano],
                             [cod_forma_pgto], [forma_pgto], [conta_central], [tipo_transacao], [taxa_financeira], [dt_upd], [desativado],
                             [usa_tef], [timestamp]) 
                            Values 
                            (@lastupdateon, @portal, @plano, @desc_plano, @qtde_parcelas, @prazo_entre_parcelas, @tipo_plano, @indice_plano,
                             @cod_forma_pgto, @forma_pgto, @conta_central, @tipo_transacao, @taxa_financeira, @dt_upd, @desativado,
                             @usa_tef, @timestamp)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPlanos - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [portal], [plano], [desc_plano], [qtde_parcelas], [prazo_entre_parcelas], [tipo_plano], [indice_plano],
                             [cod_forma_pgto], [forma_pgto], [conta_central], [tipo_transacao], [taxa_financeira], [dt_upd], [desativado],
                             [usa_tef], [timestamp]) 
                            Values 
                            (@lastupdateon, @portal, @plano, @desc_plano, @qtde_parcelas, @prazo_entre_parcelas, @tipo_plano, @indice_plano,
                             @cod_forma_pgto, @forma_pgto, @conta_central, @tipo_transacao, @taxa_financeira, @dt_upd, @desativado,
                             @usa_tef, @timestamp)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    conn.ExecuteAsync(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPlanos - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }
    }
}


using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Models.Ecommerce;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Repositorys.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace BloomersMicrovixIntegrations.Repositorys.Ecommerce
{
    public class B2CConsultaStatusRepository<T1> : IB2CConsultaStatusRepository<T1> where T1 : B2CConsultaStatus, new()
    {
        private readonly ISQLServerConnection _conn;

        public B2CConsultaStatusRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

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
                    table.Rows.Add(registros[i].lastupdateon, registros[i].id_status, registros[i].descricao_status, registros[i].timestamp, registros[i].portal);
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
                throw new Exception($"B2CConsultaStatus - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
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
                throw new Exception($"B2CConsultaStatus - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"B2CConsultaStatus - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
            }
        }

        public async Task<string> GetParameters(string tableName, string parameterCol)
        {
            string sql = $@"SELECT {parameterCol} FROM [BLOOMERS_LINX].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryFirstAsync<String>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaStatus - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public string GetParametersSync(string tableName, string parameterCol)
        {
            string sql = $@"SELECT {parameterCol} FROM [BLOOMERS_LINX].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return conn.QueryFirst<String>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaStatus - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<List<T1>> GetRegistersExists(List<T1> registros, string? tableName, string? db)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].id_status}'";
                else
                    identificadores += $"'{registros[i].id_status}', ";
            }
            string query = $"SELECT id_status, timestamp FROM [{db}].[dbo].[{tableName}_TRUSTED] WHERE id_status IN ({identificadores})";

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

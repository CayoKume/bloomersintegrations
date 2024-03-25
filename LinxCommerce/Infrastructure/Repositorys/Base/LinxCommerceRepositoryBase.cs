using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys.Base
{
    public class LinxCommerceRepositoryBase<TEntity> : ILinxCommerceRepositoryBase<TEntity> where TEntity : class, new()
    {
        private readonly ISQLServerConnection _conn;

        public LinxCommerceRepositoryBase(ISQLServerConnection conn) =>
            _conn = conn;

        public void BulkInsertIntoTableRaw(DataTable dataTable, string? database, string tableName, int dataTableRowsNumber)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    using var bulkCopy = new SqlBulkCopy((SqlConnection)conn);
                    bulkCopy.DestinationTableName = $"{database}.[dbo].{tableName}_raw";
                    bulkCopy.BatchSize = dataTableRowsNumber;
                    bulkCopy.BulkCopyTimeout = 360;
                    bulkCopy.WriteToServer(dataTable);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
            }
        }

        public async Task CallDbProcMerge(string? procName, string? tableName, string? database)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    await conn.ExecuteAsync($"{database}.[dbo].{procName}", commandTimeout: 360, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
            }
        }

        public void CallDbProcMergeSync(string? procName, string? tableName, string? database)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    conn.Execute($"{database}.[dbo].{procName}", commandTimeout: 360, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
            }
        }

        public async Task<string> GetParameters(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.QueryFirstAsync<string>(sql: sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public string GetParametersSync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    return conn.QueryFirst<string>(sql: sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<TEntity?> GetRegisterExists(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<TEntity>(sql: sql, commandTimeout: 360);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetRegisterExists - Erro ao verificar se o registro já existe na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<List<TEntity>> GetRegistersExists(string? tableName, string? sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<TEntity>(sql: sql, commandTimeout: 360);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetRegistersExists - Erro ao verificar se o registro já existe na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task IntegraRegistros(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    await conn.ExecuteAsync(sql: sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public void IntegraRegistrosSync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    conn.Execute(sql: sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - InsereRegistroIndividualSync - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }
    }
}

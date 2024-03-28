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
                using (var conn = _conn.GetDbConnection())
                {
                    using var bulkCopy = new SqlBulkCopy(conn);
                    bulkCopy.DestinationTableName = $"[{database}].[dbo].[{tableName}_raw]";
                    bulkCopy.BatchSize = dataTableRowsNumber;
                    bulkCopy.BulkCopyTimeout = 360;
                    bulkCopy.WriteToServer(dataTable);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
            }
        }

        public async Task<int> GetParameters(string sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.QueryFirstAsync<int>(sql: sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxAPIParam - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
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

        public DataTable CreateDataTable(string tableName, List<string> properties)
        {
            try
            {
                var dataTable = new DataTable(tableName);
                for (int i = 0; i < properties.Count(); i++)
                {
                    dataTable.Columns.Add(properties[i]);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - CreateDataTable - Erro ao criar a datatable {tableName} - {ex.Message}");
            }
        }
    }
}

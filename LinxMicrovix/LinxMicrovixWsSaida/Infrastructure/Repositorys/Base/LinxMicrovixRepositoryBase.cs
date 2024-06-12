using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base
{
    public class LinxMicrovixRepositoryBase<TEntity> : ILinxMicrovixRepositoryBase<TEntity> where TEntity : class, new()
    {
        private readonly ISQLServerConnection _conn;

        public LinxMicrovixRepositoryBase(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public void BulkInsertIntoTableRaw(DataTable dataTable, string database, string tableName, int dataTableRowsNumber)
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

        public async Task CallDbProcMergeAsync(string procName, string tableName, string database)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync($"{database}..{procName}", commandType: CommandType.StoredProcedure, commandTimeout: 2700);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - CallDbProcMergeAsync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
            }
        }

        public void CallDbProcMergeNotAsync(string procName, string tableName, string database)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    conn.Execute($"{database}..{procName}", commandType: CommandType.StoredProcedure, commandTimeout: 2700);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - CallDbProcMergeNotAsync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
            }
        }

        public DataTable CreateDataTable(string tableName, PropertyInfo[] properties)
        {
            try
            {
                var dataTable = new DataTable(tableName);
                for (int i = 0; i < properties.Count(); i++)
                {
                    if (properties[i].Name == "identificador_microvix" || properties[i].Name == "identificador")
                        dataTable.Columns.Add($"{properties[i].Name}", typeof(Guid));
                    else
                        dataTable.Columns.Add($"{properties[i].Name}");
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - CreateDataTable - Erro ao criar a datatable {tableName} - {ex.Message}");
            }
        }

        public async Task<IEnumerable<String>> GetCodDepositosAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryAsync<String>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetCodDepositosAsync - Erro ao obter os codigos de depositos da tabela LinxProdutosDepositos_trusted, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public IEnumerable<String> GetCodDepositosNotAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return conn.Query<String>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetCodDepositosNotAsync - Erro ao obter os codigos de depositos da tabela LinxProdutosDepositos_trusted, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryAsync<Company>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetCompanysAsync - Erro ao obter as empresas da tabela LinxLojas_trusted, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return conn.Query<Company>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetCompanysNotAsync - Erro ao obter as empresas da tabela LinxLojas_trusted, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<String> GetParametersAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.QueryFirstAsync<String>(sql: sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetParametersAsync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public String GetParametersNotAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    return conn.QueryFirst<String>(sql: sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetParametersNotAsync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<TEntity?> GetRegisterExistsAsync(string tableName, string sql)
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
                throw new Exception($"{tableName} - GetRegisterExistsAsync - Erro ao verificar se o registro já existe na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public TEntity? GetRegisterExistsNotAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = conn.Query<TEntity>(sql: sql, commandTimeout: 360);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetRegisterExistsNotAsync - Erro ao verificar se o registro já existe na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<List<TEntity>> GetRegistersExistsAsync(string tableName, string sql)
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
                throw new Exception($"{tableName} - GetRegistersExistsAsync - Erro ao verificar se o registro já existe na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public List<TEntity> GetRegistersExistsNotAsync(string? tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = conn.Query<TEntity>(sql: sql, commandTimeout: 360);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetRegistersExistsNotAsync - Erro ao verificar se o registro já existe na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<string> GetTableLastTimestampAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryFirstAsync<string>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetTableLastTimestampAsync - Erro ao obter o timestamp (MIN) da tabela {tableName}, atraves do sql {sql} - {ex.Message}");
            }
        }

        public string GetTableLastTimestampNotAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return conn.QueryFirst<string>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetTableLastTimestampNotAsync - Erro ao obter o timestamp (MIN) da tabela {tableName}, atraves do sql {sql} - {ex.Message}");
            }
        }

        public async Task InsereRegistroIndividualAsync(string tableName, string sql, object registro)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    await conn.ExecuteAsync(sql: sql, registro, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - InsereRegistroIndividualAsync - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public void InsereRegistroIndividualNotAsync(string tableName, string sql, object registro)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    conn.Execute(sql: sql, registro, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - InsereRegistroIndividualNotAsync - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<IEnumerable<String>> GetIdTabelaPrecoAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return await conn.QueryAsync<String>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetIdTabelaPrecoAsync - Erro ao obter codigos das tabelas de produtos da tabela LinxProdutosTabelas_trusted, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public IEnumerable<String> GetIdTabelaPrecoNotAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    return conn.Query<String>(sql: sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetIdTabelaPrecoNotAsync - Erro ao obter codigos das tabelas de produtos da tabela LinxProdutosTabelas_trusted, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<IEnumerable<string>> GetProductsAsync(string tableName, string sql)
        {
            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.QueryAsync<string>(sql: sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{tableName} - GetProductsAsync - Erro ao obter produtos da tabela LinxProdutos_trusted, atraves do sql: {sql} - {ex.Message}");
            }
        }
    }
}

using Dapper;
using System.Data;
using System.Data.SqlClient;
using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using Microvix.Models;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys
{
    public class LinxProdutosDepositosRepository<T1> : ILinxProdutosDepositosRepository<T1> where T1 : LinxProdutosDepositos, new()
    {
        private readonly ISQLServerConnection _conn;

        public LinxProdutosDepositosRepository(ISQLServerConnection conn) =>
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
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cod_deposito, registros[i].nome_deposito, registros[i].disponivel,
                                   registros[i].disponivel_transferencia, registros[i].timestamp, registros[i].outlet);
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
                throw new Exception($"LinxProdutosDepositos - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
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
                throw new Exception($"LinxProdutosDepositos - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxProdutosDepositos - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxProdutosDepositos - GetEmpresas - Erro ao obter as empresas da tabela LinxLojas_trusted, atraves do sql: {sql} - {ex.Message}");
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
                throw new Exception($"LinxProdutosDepositos - GetEmpresasSync - Erro ao obter as empresas da tabela LinxLojas_trusted, atraves do sql: {sql} - {ex.Message}");
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
                throw new Exception($"LinxProdutosDepositos - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
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
                throw new Exception($"LinxProdutosDepositos - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<List<T1>> GetRegistersExists(List<T1> registros, string? tableName, string? db)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cod_deposito}'";
                else
                    identificadores += $"'{registros[i].cod_deposito}', ";
            }
            string query = $"SELECT cod_deposito, timestamp FROM {db}.[dbo].{tableName} WHERE cod_deposito IN ({identificadores})";

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
                            ([lastupdateon], [portal], [cod_deposito], [nome_deposito], [disponivel], [disponivel_transferencia], [timestamp], [outlet]) 
                            Values 
                            (@lastupdateon, @portal, @cod_deposito, @nome_deposito, @disponivel, @disponivel_transferencia, @timestamp, @outlet)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosDepositos - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_deposito], [nome_deposito], [disponivel], [disponivel_transferencia], [timestamp], [outlet]) 
                            Values 
                            (@lastupdateon, @portal, @cod_deposito, @nome_deposito, @disponivel, @disponivel_transferencia, @timestamp, @outlet)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    conn.Execute(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosDepositos - InsereRegistroIndividualSync - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }
    }
}

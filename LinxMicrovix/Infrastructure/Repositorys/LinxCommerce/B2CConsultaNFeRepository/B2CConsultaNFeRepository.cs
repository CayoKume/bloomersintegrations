using Dapper;
using System.Data;
using System.Data.SqlClient;
using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Models.Ecommerce;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Repositorys.Interfaces;

namespace BloomersMicrovixIntegrations.Repositorys.Ecommerce
{
    public class B2CConsultaNFeRepository<T1> : IB2CConsultaNFeRepository<T1> where T1 : B2CConsultaNFe
    {
        private readonly ISQLServerConnection _conn;

        public B2CConsultaNFeRepository(ISQLServerConnection conn) =>
            _conn = conn;

        public void BulkInsertIntoTableRaw(List<T1> notasFiscais, string? tableName, string? db)
        {
            try
            {
                var table = new DataTable();
                var properties = notasFiscais[0].GetType().GetProperties();

                for (int i = 0; i < properties.Count(); i++)
                {
                    if (properties[i].Name == "identificador_microvix")
                        table.Columns.Add($"{properties[i].Name}", typeof(Guid));
                    else
                        table.Columns.Add($"{properties[i].Name}");
                }

                for (int i = 0; i < notasFiscais.Count(); i++)
                {
                    table.Rows.Add(notasFiscais[i].lastupdateon, notasFiscais[i].id_nfe, notasFiscais[i].id_pedido, notasFiscais[i].documento, notasFiscais[i].data_emissao, notasFiscais[i].chave_nfe, notasFiscais[i].situacao,
                                   notasFiscais[i].xml, notasFiscais[i].excluido, notasFiscais[i].identificador_microvix, notasFiscais[i].dt_insert, notasFiscais[i].valor_nota, notasFiscais[i].serie, notasFiscais[i].frete,
                                   notasFiscais[i].timestamp, notasFiscais[i].portal, notasFiscais[i].nProt, notasFiscais[i].codigo_modelo_nf, notasFiscais[i].justificativa);
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
                throw new Exception($"B2CConsultaNFe - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
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
                throw new Exception($"B2CConsultaNFe - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"B2CConsultaNFe - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"B2CConsultaNFe - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atrves do sql: {sql} - {ex.Message}");
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
                throw new Exception($"B2CConsultaNFe - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atrves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<List<T1>> GetRegistersExists(List<T1> registros, string? tableName, string? db)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].chave_nfe}'";
                else
                    identificadores += $"'{registros[i].chave_nfe}', ";
            }
            string query = $"SELECT CHAVE_NFE, TIMESTAMP FROM [{db}].[dbo].[{tableName}_TRUSTED] WHERE CHAVE_NFE IN ({identificadores})";

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

        public async Task InsereRegistroIndividual(T1 NFe, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [id_nfe], [id_pedido], [documento], [data_emissao], [chave_nfe],
                             [situacao], [xml], [excluido], [identificador_microvix], [dt_insert], [valor_nota],
                             [serie], [frete], [timestamp], [portal], [nProt], [codigo_modelo_nf], [justificativa]) 
                            Values 
                            (@lastupdateon, @id_nfe, @id_pedido, @documento, @data_emissao, @chave_nfe,
                             @situacao, @xml, @excluido, @identificador_microvix, @dt_insert, @valor_nota,
                             @serie, @frete, @timestamp, @portal, @nProt, @codigo_modelo_nf, @justificativa)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, NFe);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaNFe - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atrves do sql: {sql} - {ex.Message}");
            }
        }

        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [id_nfe], [id_pedido], [documento], [data_emissao], [chave_nfe],
                             [situacao], [xml], [excluido], [identificador_microvix], [dt_insert], [valor_nota],
                             [serie], [frete], [timestamp], [portal], [nProt], [codigo_modelo_nf], [justificativa]) 
                            Values 
                            (@lastupdateon, @id_nfe, @id_pedido, @documento, @data_emissao, @chave_nfe,
                             @situacao, @xml, @excluido, @identificador_microvix, @dt_insert, @valor_nota,
                             @serie, @frete, @timestamp, @portal, @nProt, @codigo_modelo_nf, @justificativa)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    conn.Execute(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaNFe - InsereRegistroIndividualSync - Erro ao inserir registro na tabela {tableName}, atrves do sql: {sql} - {ex.Message}");
            }
        }
    }
}

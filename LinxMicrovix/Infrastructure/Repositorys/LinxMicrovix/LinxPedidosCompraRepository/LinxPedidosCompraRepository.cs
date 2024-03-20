using Dapper;
using System.Data;
using System.Data.SqlClient;
using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using Microvix.Models;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys
{
    public class LinxPedidosCompraRepository<T1> : ILinxPedidosCompraRepository<T1> where T1 : LinxPedidosCompra, new()
    {
        private readonly ISQLServerConnection _conn;

        public LinxPedidosCompraRepository(ISQLServerConnection conn) =>
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
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cnpj_emp, registros[i].cod_pedido, registros[i].data_pedido,
                                   registros[i].transacao, registros[i].usuario, registros[i].codigo_fornecedor, registros[i].cod_produto, registros[i].quantidade,
                                   registros[i].valor_unitario, registros[i].cod_comprador, registros[i].valor_frete, registros[i].valor_total, registros[i].cod_plano_pagamento,
                                   registros[i].plano_pagamento, registros[i].obs, registros[i].aprovado, registros[i].cancelado, registros[i].encerrado, registros[i].data_aprovacao,
                                   registros[i].numero_ped_fornec, registros[i].tipo_frete, registros[i].natureza_operacao, registros[i].previsao_entrega, registros[i].numero_projeto_officina,
                                   registros[i].status_pedido, registros[i].qtde_entregue, registros[i].descricao_frete, registros[i].integrado_linx, registros[i].nf_gerada, registros[i].timestamp,
                                   registros[i].empresa);
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
                throw new Exception($"LinxPedidosCompra - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
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
                throw new Exception($"LinxPedidosCompra - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxPedidosCompra - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxPedidosCompra - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql {sql} - {ex.Message}");
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
                throw new Exception($"LinxPedidosCompra - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql {sql} - {ex.Message}");
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
                throw new Exception($"LinxPedidosCompra - GetEmpresas - Erro ao obter as empresas da tabela LinxLojas_trusted, atraves do sql {sql} - {ex.Message}");
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
                throw new Exception($"LinxPedidosCompra - GetEmpresasSync - Erro ao obter as empresas da tabela LinxLojas_trusted, atraves do sql {sql} - {ex.Message}");
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
            string query = $"SELECT cnpj_emp, cod_produto, cod_pedido, TIMESTAMP FROM {db}.[dbo].{tableName} WHERE cod_pedido IN ({identificadores})";

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

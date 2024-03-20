using Dapper;
using System.Data;
using System.Data.SqlClient;
using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Models.Ecommerce;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Core.Interfaces;

namespace BloomersMicrovixIntegrations.Repositorys.Ecommerce
{
    public class B2CConsultaPedidosRepository<T1> : IB2CConsultaPedidosRepository<T1> where T1 : B2CConsultaPedidos, new()
    {
        private readonly ISQLServerConnection _conn;

        public B2CConsultaPedidosRepository(ISQLServerConnection conn) =>
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
                    table.Rows.Add(registros[i].lastupdateon, registros[i].id_pedido, registros[i].dt_pedido.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].cod_cliente_erp, registros[i].cod_cliente_b2c, registros[i].vl_frete, registros[i].forma_pgto, registros[i].plano_pagamento,
                        registros[i].anotacao, registros[i].taxa_impressao, registros[i].finalizado, registros[i].valor_frete_gratis, registros[i].tipo_frete, registros[i].id_status, registros[i].cod_transportador,
                        registros[i].tipo_cobranca_frete, registros[i].ativo, registros[i].empresa, registros[i].id_tabela_preco, registros[i].valor_credito, registros[i].cod_vendedor, registros[i].timestamp, registros[i].dt_insert.ToString("yyyy-MM-dd HH:mm:ss"),
                        registros[i].dt_disponivel_faturamento.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].portal, registros[i].mensagem_falha_faturamento, registros[i].id_tipo_b2c, registros[i].ecommerce_origem, registros[i].order_id, registros[i].fulfillment_id);
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
                throw new Exception($"B2CConsultaPedidos - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
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
                throw new Exception($"B2CConsultaPedidos - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"B2CConsultaPedidos - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"B2CConsultaPedidos - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
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
                throw new Exception($"B2CConsultaPedidos - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public async Task<List<T1>> GetRegistersExists(List<T1> registros, string? tableName, string? db)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].order_id}'";
                else
                    identificadores += $"'{registros[i].order_id}', ";
            }
            string query = $"SELECT order_id, timestamp FROM [{db}].[dbo].[{tableName}_TRUSTED] WHERE order_id IN ({identificadores})";

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
                            ([lastupdateon], [id_pedido], [dt_pedido], [cod_cliente_erp], [cod_cliente_b2c], [vl_frete], [forma_pgto], [plano_pagamento], [anotacao], [taxa_impressao], [finalizado], [valor_frete_gratis], 
                             [tipo_frete], [id_status], [cod_transportador], [tipo_cobranca_frete], [ativo], [empresa], [id_tabela_preco], [valor_credito], [cod_vendedor], [timestamp], [dt_insert], [dt_disponivel_faturamento], 
                             [portal], [mensagem_falha_faturamento], [id_tipo_b2c], [ecommerce_origem], [order_id], [fulfillment_id]) 
                            Values 
                            (@lastupdateon, @id_pedido, @dt_pedido, @cod_cliente_erp, @cod_cliente_b2c, @vl_frete, @forma_pgto, @plano_pagamento, @anotacao, @taxa_impressao, @finalizado, @valor_frete_gratis, 
                             @tipo_frete, @id_status, @cod_transportador, @tipo_cobranca_frete, @ativo, @empresa, @id_tabela_preco, @valor_credito, @cod_vendedor, @timestamp, @dt_insert, @dt_disponivel_faturamento, 
                             @portal, @mensagem_falha_faturamento, @id_tipo_b2c, @ecommerce_origem, @order_id, @fulfillment_id)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaPedidos - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }

        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [id_pedido], [dt_pedido], [cod_cliente_erp], [cod_cliente_b2c], [vl_frete], [forma_pgto], [plano_pagamento], [anotacao], [taxa_impressao], [finalizado], [valor_frete_gratis], 
                             [tipo_frete], [id_status], [cod_transportador], [tipo_cobranca_frete], [ativo], [empresa], [id_tabela_preco], [valor_credito], [cod_vendedor], [timestamp], [dt_insert], [dt_disponivel_faturamento], 
                             [portal], [mensagem_falha_faturamento], [id_tipo_b2c], [ecommerce_origem], [order_id], [fulfillment_id]) 
                            Values 
                            (@lastupdateon, @id_pedido, @dt_pedido, @cod_cliente_erp, @cod_cliente_b2c, @vl_frete, @forma_pgto, @plano_pagamento, @anotacao, @taxa_impressao, @finalizado, @valor_frete_gratis, 
                             @tipo_frete, @id_status, @cod_transportador, @tipo_cobranca_frete, @ativo, @empresa, @id_tabela_preco, @valor_credito, @cod_vendedor, @timestamp, @dt_insert, @dt_disponivel_faturamento, 
                             @portal, @mensagem_falha_faturamento, @id_tipo_b2c, @ecommerce_origem, @order_id, @fulfillment_id)";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    conn.Execute(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaPedidos - InsereRegistroIndividualSync - Erro ao inserir registro na tabela {tableName}, atraves do sql: {sql} - {ex.Message}");
            }
        }
    }
}

using Dapper;
using System.Data;
using System.Data.SqlClient;
using BloomersGeneralConnection.interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using Microvix.Models;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys
{
    public class LinxMovimentoRepository<T1> : ILinxMovimentoRepository<T1> where T1 : LinxMovimento, new()
    {
        private readonly ISQLServerConnection _conn;

        public LinxMovimentoRepository(ISQLServerConnection conn) =>
            _conn = conn;

        public void BulkInsertIntoTableRaw(List<T1> registros, string? tableName, string? db)
        {
            try
            {
                var table = new DataTable();
                var properties = registros[0].GetType().GetProperties();

                for (int i = 0; i < properties.Count(); i++)
                {
                    if (properties[i].Name == "identificador")
                        table.Columns.Add($"{properties[i].Name}", typeof(Guid));
                    else
                        table.Columns.Add($"{properties[i].Name}");
                }

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cnpj_emp, registros[i].transacao, registros[i].usuario, registros[i].documento, registros[i].chave_nf,
                                   registros[i].ecf, registros[i].numero_serie_ecf, registros[i].modelo_nf, registros[i].data_documento, registros[i].data_lancamento, registros[i].codigo_cliente,
                                   registros[i].serie, registros[i].desc_cfop, registros[i].id_cfop, registros[i].cod_vendedor, registros[i].quantidade, registros[i].preco_custo, registros[i].valor_liquido,
                                   registros[i].desconto, registros[i].cst_icms, registros[i].cst_pis, registros[i].cst_cofins, registros[i].cst_ipi, registros[i].valor_icms, registros[i].aliquota_icms,
                                   registros[i].base_icms, registros[i].valor_pis, registros[i].aliquota_pis, registros[i].base_pis, registros[i].valor_cofins, registros[i].aliquota_cofins, registros[i].base_cofins,
                                   registros[i].valor_icms_st, registros[i].aliquota_icms_st, registros[i].base_icms_st, registros[i].valor_ipi, registros[i].aliquota_ipi, registros[i].base_ipi, registros[i].valor_total,
                                   registros[i].forma_dinheiro, registros[i].total_dinheiro, registros[i].forma_cheque, registros[i].total_cheque, registros[i].forma_cartao, registros[i].total_cartao, registros[i].forma_crediario,
                                   registros[i].total_crediario, registros[i].forma_convenio, registros[i].total_convenio, registros[i].frete, registros[i].operacao, registros[i].tipo_transacao, registros[i].cod_produto,
                                   registros[i].cod_barra, registros[i].cancelado, registros[i].excluido, registros[i].soma_relatorio, registros[i].identificador, registros[i].deposito, registros[i].obs, registros[i].preco_unitario,
                                   registros[i].hora_lancamento, registros[i].natureza_operacao, registros[i].tabela_preco, registros[i].nome_tabela_preco, registros[i].cod_sefaz_situacao, registros[i].desc_sefaz_situacao,
                                   registros[i].protocolo_aut_nfe, registros[i].dt_update, registros[i].forma_cheque_prazo, registros[i].total_cheque_prazo, registros[i].cod_natureza_operacao, registros[i].preco_tabela_epoca,
                                   registros[i].desconto_total_item, registros[i].conferido, registros[i].transacao_pedido_venda, registros[i].codigo_modelo_nf, registros[i].acrescimo, registros[i].mob_checkout, registros[i].aliquota_iss,
                                   registros[i].base_iss, registros[i].ordem, registros[i].codigo_rotina_origem, registros[i].timestamp, registros[i].troco, registros[i].transportador, registros[i].icms_aliquota_desonerado,
                                   registros[i].icms_valor_desonerado_item, registros[i].empresa, registros[i].desconto_item, registros[i].aliq_iss, registros[i].iss_base_item, registros[i].despesas, registros[i].seguro_total_item,
                                   registros[i].acrescimo_total_item, registros[i].despesas_total_item, registros[i].forma_pix, registros[i].total_pix, registros[i].forma_deposito_bancario, registros[i].total_deposito_bancario);
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
                throw new Exception($"LinxMovimento - BulkInsertIntoTableRaw - Erro ao realizar BULK INSERT na tabela {tableName} - {ex.Message}");
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
                throw new Exception($"LinxMovimento - CallDbProcMerge - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxMovimento - CallDbProcMergeSync - Erro ao realizar merge na tabela {tableName}, através da proc : {procName} - {ex.Message}");
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
                throw new Exception($"LinxMovimento - GetParameters - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql {sql} - {ex.Message}");
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
                throw new Exception($"LinxMovimento - GetParametersSync - Erro ao obter parametros dos filtros da tabela LinxAPIParam, atraves do sql {sql} - {ex.Message}");
            }
        }

        public async Task InsereRegistroIndividual(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [transacao], [usuario], [documento], [chave_nf],
                             [ecf], [numero_serie_ecf], [modelo_nf], [data_documento], [data_lancamento], [codigo_cliente],
                             [serie], [desc_cfop], [id_cfop], [cod_vendedor], [quantidade], [preco_custo], [valor_liquido],
                             [desconto], [cst_icms], [cst_pis], [cst_cofins], [cst_ipi], [valor_icms], [aliquota_icms],
                             [base_icms], [valor_pis], [aliquota_pis], [base_pis], [valor_cofins], [aliquota_cofins], [base_cofins],
                             [valor_icms_st], [aliquota_icms_st], [base_icms_st], [valor_ipi], [aliquota_ipi], [base_ipi], [valor_total],
                             [forma_dinheiro], [total_dinheiro], [forma_cheque], [total_cheque], [forma_cartao], [total_cartao], [forma_crediario],
                             [total_crediario], [forma_convenio], [total_convenio], [frete], [operacao], [tipo_transacao], [cod_produto],
                             [cod_barra], [cancelado],[excluido], [soma_relatorio], [identificador], [deposito], [obs], [preco_unitario],
                             [hora_lancamento], [natureza_operacao], [tabela_preco], [nome_tabela_preco], [cod_sefaz_situacao], [desc_sefaz_situacao],
                             [protocolo_aut_nfe], [dt_update], [forma_cheque_prazo], [total_cheque_prazo], [cod_natureza_operacao], [preco_tabela_epoca],
                             [desconto_total_item], [conferido], [transacao_pedido_venda], [codigo_modelo_nf], [acrescimo], [mob_checkout], [aliquota_iss],
                             [base_iss], [ordem], [codigo_rotina_origem], [timestamp], [troco], [transportador], [icms_aliquota_desonerado],
                             [icms_valor_desonerado_item], [empresa], [desconto_item], [aliq_iss], [iss_base_item], [despesas], [seguro_total_item],
                             [acrescimo_total_item], [despesas_total_item], [forma_pix], [total_pix], [forma_deposito_bancario],[total_deposito_bancario]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @transacao, @usuario, @documento, @chave_nf,
                             @ecf, @numero_serie_ecf, @modelo_nf, @data_documento, @data_lancamento, @codigo_cliente,
                             @serie, @desc_cfop, @id_cfop, @cod_vendedor, @quantidade, @preco_custo, @valor_liquido,
                             @desconto, @cst_icms, @cst_pis, @cst_cofins, @cst_ipi, @valor_icms, @aliquota_icms,
                             @base_icms, @valor_pis, @aliquota_pis, @base_pis, @valor_cofins, @aliquota_cofins, @base_cofins,
                             @valor_icms_st, @aliquota_icms_st, @base_icms_st, @valor_ipi, @aliquota_ipi, @base_ipi, @valor_total,
                             @forma_dinheiro, @total_dinheiro, @forma_cheque, @total_cheque, @forma_cartao, @total_cartao, @forma_crediario,
                             @total_crediario, @forma_convenio, @total_convenio, @frete, @operacao, @tipo_transacao, @cod_produto,
                             @cod_barra, @cancelado,@excluido, @soma_relatorio, @identificador, @deposito, @obs, @preco_unitario,
                             @hora_lancamento, @natureza_operacao, @tabela_preco, @nome_tabela_preco, @cod_sefaz_situacao, @desc_sefaz_situacao,
                             @protocolo_aut_nfe, @dt_update, @forma_cheque_prazo, @total_cheque_prazo, @cod_natureza_operacao, @preco_tabela_epoca,
                             @desconto_total_item, @conferido, @transacao_pedido_venda, @codigo_modelo_nf, @acrescimo, @mob_checkout, @aliquota_iss,
                             @base_iss, @ordem, @codigo_rotina_origem, @timestamp, @troco, @transportador, @icms_aliquota_desonerado,
                             @icms_valor_desonerado_item, @empresa, @desconto_item, @aliq_iss, @iss_base_item, @despesas, @seguro_total_item,
                             @acrescimo_total_item, @despesas_total_item, @forma_pix, @total_pix, @forma_deposito_bancario,@total_deposito_bancario
                            )";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxMovimento - InsereRegistroIndividual - Erro ao inserir registro na tabela {tableName}, atraves do sql {sql} - {ex.Message}");
            }
        }

        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db)
        {
            string sql = @$"INSERT INTO {db}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [transacao], [usuario], [documento], [chave_nf],
                             [ecf], [numero_serie_ecf], [modelo_nf], [data_documento], [data_lancamento], [codigo_cliente],
                             [serie], [desc_cfop], [id_cfop], [cod_vendedor], [quantidade], [preco_custo], [valor_liquido],
                             [desconto], [cst_icms], [cst_pis], [cst_cofins], [cst_ipi], [valor_icms], [aliquota_icms],
                             [base_icms], [valor_pis], [aliquota_pis], [base_pis], [valor_cofins], [aliquota_cofins], [base_cofins],
                             [valor_icms_st], [aliquota_icms_st], [base_icms_st], [valor_ipi], [aliquota_ipi], [base_ipi], [valor_total],
                             [forma_dinheiro], [total_dinheiro], [forma_cheque], [total_cheque], [forma_cartao], [total_cartao], [forma_crediario],
                             [total_crediario], [forma_convenio], [total_convenio], [frete], [operacao], [tipo_transacao], [cod_produto],
                             [cod_barra], [cancelado],[excluido], [soma_relatorio], [identificador], [deposito], [obs], [preco_unitario],
                             [hora_lancamento], [natureza_operacao], [tabela_preco], [nome_tabela_preco], [cod_sefaz_situacao], [desc_sefaz_situacao],
                             [protocolo_aut_nfe], [dt_update], [forma_cheque_prazo], [total_cheque_prazo], [cod_natureza_operacao], [preco_tabela_epoca],
                             [desconto_total_item], [conferido], [transacao_pedido_venda], [codigo_modelo_nf], [acrescimo], [mob_checkout], [aliquota_iss],
                             [base_iss], [ordem], [codigo_rotina_origem], [timestamp], [troco], [transportador], [icms_aliquota_desonerado],
                             [icms_valor_desonerado_item], [empresa], [desconto_item], [aliq_iss], [iss_base_item], [despesas], [seguro_total_item],
                             [acrescimo_total_item], [despesas_total_item], [forma_pix], [total_pix], [forma_deposito_bancario],[total_deposito_bancario]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @transacao, @usuario, @documento, @chave_nf,
                             @ecf, @numero_serie_ecf, @modelo_nf, @data_documento, @data_lancamento, @codigo_cliente,
                             @serie, @desc_cfop, @id_cfop, @cod_vendedor, @quantidade, @preco_custo, @valor_liquido,
                             @desconto, @cst_icms, @cst_pis, @cst_cofins, @cst_ipi, @valor_icms, @aliquota_icms,
                             @base_icms, @valor_pis, @aliquota_pis, @base_pis, @valor_cofins, @aliquota_cofins, @base_cofins,
                             @valor_icms_st, @aliquota_icms_st, @base_icms_st, @valor_ipi, @aliquota_ipi, @base_ipi, @valor_total,
                             @forma_dinheiro, @total_dinheiro, @forma_cheque, @total_cheque, @forma_cartao, @total_cartao, @forma_crediario,
                             @total_crediario, @forma_convenio, @total_convenio, @frete, @operacao, @tipo_transacao, @cod_produto,
                             @cod_barra, @cancelado,@excluido, @soma_relatorio, @identificador, @deposito, @obs, @preco_unitario,
                             @hora_lancamento, @natureza_operacao, @tabela_preco, @nome_tabela_preco, @cod_sefaz_situacao, @desc_sefaz_situacao,
                             @protocolo_aut_nfe, @dt_update, @forma_cheque_prazo, @total_cheque_prazo, @cod_natureza_operacao, @preco_tabela_epoca,
                             @desconto_total_item, @conferido, @transacao_pedido_venda, @codigo_modelo_nf, @acrescimo, @mob_checkout, @aliquota_iss,
                             @base_iss, @ordem, @codigo_rotina_origem, @timestamp, @troco, @transportador, @icms_aliquota_desonerado,
                             @icms_valor_desonerado_item, @empresa, @desconto_item, @aliq_iss, @iss_base_item, @despesas, @seguro_total_item,
                             @acrescimo_total_item, @despesas_total_item, @forma_pix, @total_pix, @forma_deposito_bancario,@total_deposito_bancario
                            )";

            try
            {
                using (var conn = _conn.GetDbConnection())
                {
                    conn.Execute(sql, registro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxMovimento - InsereRegistroIndividualSync - Erro ao inserir registro na tabela {tableName}, atraves do sql {sql} - {ex.Message}");
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
                throw new Exception($"LinxMovimento - GetEmpresas - Erro ao obter as empresas da tabela LinxLojas_trusted, atraves do sql {sql} - {ex.Message}");
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
                throw new Exception($"LinxMovimento - GetEmpresasSync - Erro ao obter as empresas da tabela LinxLojas_trusted, atraves do sql {sql} - {ex.Message}");
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
            string query = $"SELECT cnpj_emp, cod_produto, chave_nf, TIMESTAMP FROM BLOOMERS_LINX..LinxMovimento_trusted WHERE chave_nf IN ({identificadores})";

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

using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxMovimentoRepository : ILinxMovimentoRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxMovimento> _linxMicrovixRepositoryBase;

        public LinxMovimentoRepository(ILinxMicrovixRepositoryBase<LinxMovimento> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxMovimento> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxMovimento().GetType().GetProperties());

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

                _linxMicrovixRepositoryBase.BulkInsertIntoTableRaw(table, database, tableName, table.Rows.Count);
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetParametersAsync(string tableName, string database, string parameterCol)
        {
            string sql = $@"SELECT {parameterCol} FROM [BLOOMERS_LINX].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

            try
            {
                return await _linxMicrovixRepositoryBase.GetParametersAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public string GetParametersNotAsync(string tableName, string database, string parameterCol)
        {
            string sql = $@"SELECT {parameterCol} FROM [BLOOMERS_LINX].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

            try
            {
                return _linxMicrovixRepositoryBase.GetParametersNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task InsereRegistroIndividualAsync(LinxMovimento registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
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
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(LinxMovimento registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
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
                _linxMicrovixRepositoryBase.InsereRegistroIndividualNotAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database)
        {
            string sql = $@"SELECT empresa as cod_company, nome_emp as name_company, cnpj_emp as doc_company FROM BLOOMERS_LINX..LinxLojas_trusted WHERE nome_emp LIKE '%MISHA%' or nome_emp LIKE '%OPEN%'";

            try
            {
                return await _linxMicrovixRepositoryBase.GetCompanysAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database)
        {
            string sql = $@"SELECT empresa as cod_company, nome_emp as name_company, cnpj_emp as doc_company FROM BLOOMERS_LINX..LinxLojas_trusted WHERE nome_emp LIKE '%MISHA%' or nome_emp LIKE '%OPEN%'";

            try
            {
                return _linxMicrovixRepositoryBase.GetCompanysNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<LinxMovimento>> GetRegistersExistsAsync(List<LinxMovimento> registros, string tableName, string database)
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
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxMovimento> GetRegistersExistsNotAsync(List<LinxMovimento> registros, string tableName, string database)
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
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }
    }
}

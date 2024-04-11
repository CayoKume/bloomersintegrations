using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxPedidosVendaRepository : ILinxPedidosVendaRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxPedidosVenda> _linxMicrovixRepositoryBase;

        public LinxPedidosVendaRepository(ILinxMicrovixRepositoryBase<LinxPedidosVenda> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxPedidosVenda> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxPedidosVenda().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cnpj_emp, registros[i].cod_pedido, registros[i].data_lancamento, registros[i].hora_lancamento,
                                   registros[i].transacao, registros[i].usuario, registros[i].codigo_cliente, registros[i].cod_produto, registros[i].quantidade, registros[i].valor_unitario,
                                   registros[i].cod_vendedor, registros[i].valor_frete, registros[i].valor_total, registros[i].desconto_item, registros[i].cod_plano_pagamento, registros[i].plano_pagamento,
                                   registros[i].obs, registros[i].aprovado, registros[i].cancelado, registros[i].data_aprovacao, registros[i].data_alteracao, registros[i].tipo_frete,
                                   registros[i].natureza_operacao, registros[i].tabela_preco, registros[i].nome_tabela_preco, registros[i].previsao_entrega, registros[i].realizado_por,
                                   registros[i].pontuacao_ser, registros[i].venda_externa, registros[i].nf_gerada, registros[i].status, registros[i].numero_projeto_officina, registros[i].cod_natureza_operacao,
                                   registros[i].margem_contribuicao, registros[i].doc_origem, registros[i].posicao_item, registros[i].orcamento_origem, registros[i].transacao_origem, registros[i].timestamp,
                                   registros[i].desconto, registros[i].transacao_ws, registros[i].empresa, registros[i].transportador, registros[i].deposito);
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

        public async Task InsereRegistroIndividualAsync(LinxPedidosVenda registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [cod_pedido], [data_lancamento], [hora_lancamento], [transacao], [usuario], [codigo_cliente], [cod_produto],
                             [quantidade], [valor_unitario], [cod_vendedor], [valor_frete], [valor_total], [desconto_item], [cod_plano_pagamento], [plano_pagamento], [obs],
                             [aprovado], [cancelado], [data_aprovacao], [data_alteracao], [tipo_frete], [natureza_operacao], [tabela_preco], [nome_tabela_preco], [previsao_entrega],
                             [realizado_por], [pontuacao_ser], [venda_externa], [nf_gerada], [status], [numero_projeto_officina], [cod_natureza_operacao], [margem_contribuicao],
                             [doc_origem], [posicao_item], [orcamento_origem], [transacao_origem], [timestamp], [desconto], [transacao_ws], [empresa], [transportador], [deposito]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @cod_pedido, @data_lancamento, @hora_lancamento, @transacao, @usuario, @codigo_cliente, @cod_produto,
                             @quantidade, @valor_unitario, @cod_vendedor, @valor_frete, @valor_total, @desconto_item, @cod_plano_pagamento, @plano_pagamento, @obs,
                             @aprovado, @cancelado, @data_aprovacao, @data_alteracao, @tipo_frete, @natureza_operacao, @tabela_preco, @nome_tabela_preco, @previsao_entrega,
                             @realizado_por, @pontuacao_ser, @venda_externa, @nf_gerada, @status, @numero_projeto_officina, @cod_natureza_operacao, @margem_contribuicao,
                             @doc_origem, @posicao_item, @orcamento_origem, @transacao_origem, @timestamp, @desconto, @transacao_ws, @empresa, @transportador, @deposito
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

        public void InsereRegistroIndividualNotAsync(LinxPedidosVenda registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [cod_pedido], [data_lancamento], [hora_lancamento], [transacao], [usuario], [codigo_cliente], [cod_produto],
                             [quantidade], [valor_unitario], [cod_vendedor], [valor_frete], [valor_total], [desconto_item], [cod_plano_pagamento], [plano_pagamento], [obs],
                             [aprovado], [cancelado], [data_aprovacao], [data_alteracao], [tipo_frete], [natureza_operacao], [tabela_preco], [nome_tabela_preco], [previsao_entrega],
                             [realizado_por], [pontuacao_ser], [venda_externa], [nf_gerada], [status], [numero_projeto_officina], [cod_natureza_operacao], [margem_contribuicao],
                             [doc_origem], [posicao_item], [orcamento_origem], [transacao_origem], [timestamp], [desconto], [transacao_ws], [empresa], [transportador], [deposito]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @cod_pedido, @data_lancamento, @hora_lancamento, @transacao, @usuario, @codigo_cliente, @cod_produto,
                             @quantidade, @valor_unitario, @cod_vendedor, @valor_frete, @valor_total, @desconto_item, @cod_plano_pagamento, @plano_pagamento, @obs,
                             @aprovado, @cancelado, @data_aprovacao, @data_alteracao, @tipo_frete, @natureza_operacao, @tabela_preco, @nome_tabela_preco, @previsao_entrega,
                             @realizado_por, @pontuacao_ser, @venda_externa, @nf_gerada, @status, @numero_projeto_officina, @cod_natureza_operacao, @margem_contribuicao,
                             @doc_origem, @posicao_item, @orcamento_origem, @transacao_origem, @timestamp, @desconto, @transacao_ws, @empresa, @transportador, @deposito
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
            string sql = $@"SELECT empresa as numero_erp_empresa, nome_emp as nome_empresa, cnpj_emp as doc_empresa FROM BLOOMERS_LINX..LinxLojas_trusted WHERE nome_emp LIKE '%MISHA%' or nome_emp LIKE '%OPEN%'";

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
            string sql = $@"SELECT empresa as numero_erp_empresa, nome_emp as nome_empresa, cnpj_emp as doc_empresa FROM BLOOMERS_LINX..LinxLojas_trusted WHERE nome_emp LIKE '%MISHA%' or nome_emp LIKE '%OPEN%'";

            try
            {
                return _linxMicrovixRepositoryBase.GetCompanysNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<LinxPedidosVenda>> GetRegistersExistsAsync(List<LinxPedidosVenda> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj_emp}'";
                else
                    identificadores += $"'{registros[i].cnpj_emp}', ";
            }
            string query = $"SELECT cnpj_emp, cod_produto, cod_pedido, timestamp FROM {database}.[dbo].{tableName} WHERE cod_pedido IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxPedidosVenda> GetRegistersExistsNotAsync(List<LinxPedidosVenda> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj_emp}'";
                else
                    identificadores += $"'{registros[i].cnpj_emp}', ";
            }
            string query = $"SELECT cnpj_emp, cod_produto, cod_pedido, timestamp FROM {database}.[dbo].{tableName} WHERE cod_pedido IN ({identificadores})";

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

using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxProdutosRepository : ILinxProdutosRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxProdutos> _linxMicrovixRepositoryBase;

        public LinxProdutosRepository(ILinxMicrovixRepositoryBase<LinxProdutos> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxProdutos> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxProdutos().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cod_produto, registros[i].cod_barra, registros[i].nome, registros[i].ncm, registros[i].cest, registros[i].referencia, registros[i].cod_auxiliar, registros[i].unidade, registros[i].desc_cor, registros[i].desc_tamanho,
                                   registros[i].desc_setor, registros[i].desc_linha, registros[i].desc_marca, registros[i].desc_colecao, registros[i].dt_update, registros[i].cod_fornecedor, registros[i].desativado, registros[i].desc_espessura, registros[i].id_espessura, registros[i].desc_classificacao,
                                   registros[i].id_classificacao, registros[i].origem_mercadoria, registros[i].peso_liquido, registros[i].peso_bruto, registros[i].id_cor, registros[i].id_tamanho, registros[i].id_setor, registros[i].id_linha, registros[i].id_marca, registros[i].id_colecao, registros[i].dt_inclusao,
                                   registros[i].timestamp, registros[i].fator_conversao, registros[i].codigo_integracao_ws, registros[i].codigo_integracao_reshop, registros[i].id_produtos_opticos_tipo, registros[i].id_sped_tipo_item, registros[i].componente, registros[i].altura_para_frete,
                                   registros[i].largura_para_frete, registros[i].comprimento_para_frete, registros[i].loja_virtual, registros[i].codigoproduto_io, registros[i].cod_comprador, registros[i].altura, registros[i].largura, registros[i].comprimento, registros[i].codigo_integracao_oms);
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

        public Task<List<LinxProdutos>> GetRegistersExistsAsync(List<LinxProdutos> registros, string tableName, string database)
        {
            throw new NotImplementedException();
        }

        public List<LinxProdutos> GetRegistersExistsNotAsync(List<LinxProdutos> registros, string tableName, string database)
        {
            throw new NotImplementedException();
        }

        public async Task InsereRegistroIndividualAsync(LinxProdutos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_produto], [cod_barra], [nome], [ncm], [cest], [referencia], [cod_auxiliar], [unidade], [desc_cor], [desc_tamanho], 
                             [desc_setor], [desc_linha], [desc_marca], [desc_colecao], [dt_update], [cod_fornecedor], [desativado], [desc_espessura], [id_espessura], [desc_classificacao],
                             [id_classificacao], [origem_mercadoria], [peso_liquido], [peso_bruto], [id_cor], [id_tamanho], [id_setor], [id_linha], [id_marca], [id_colecao], [dt_inclusao],
                             [timestamp], [fator_conversao], [codigo_integracao_ws], [codigo_integracao_reshop], [id_produtos_opticos_tipo], [id_sped_tipo_item], [componente], [altura_para_frete],
                             [largura_para_frete], [comprimento_para_frete], [loja_virtual], [codigoproduto_io], [cod_comprador], [altura], [largura], [comprimento], [codigo_integracao_oms]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cod_produto, @cod_barra, @nome, @ncm, @cest, @referencia, @cod_auxiliar, @unidade, @desc_cor, @desc_tamanho, 
                             @desc_setor, @desc_linha, @desc_marca, @desc_colecao, @dt_update, @cod_fornecedor, @desativado, @desc_espessura, @id_espessura, @desc_classificacao,
                             @id_classificacao, @origem_mercadoria, @peso_liquido, @peso_bruto, @id_cor, @id_tamanho, @id_setor, @id_linha, @id_marca, @id_colecao, @dt_inclusao,
                             @timestamp, @fator_conversao, @codigo_integracao_ws, @codigo_integracao_reshop, @id_produtos_opticos_tipo, @id_sped_tipo_item, @componente, @altura_para_frete,
                             @largura_para_frete, @comprimento_para_frete, @loja_virtual, @codigoproduto_io, @cod_comprador, @altura, @largura, @comprimento, @codigo_integracao_oms
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

        public void InsereRegistroIndividualNotAsync(LinxProdutos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_produto], [cod_barra], [nome], [ncm], [cest], [referencia], [cod_auxiliar], [unidade], [desc_cor], [desc_tamanho], 
                             [desc_setor], [desc_linha], [desc_marca], [desc_colecao], [dt_update], [cod_fornecedor], [desativado], [desc_espessura], [id_espessura], [desc_classificacao],
                             [id_classificacao], [origem_mercadoria], [peso_liquido], [peso_bruto], [id_cor], [id_tamanho], [id_setor], [id_linha], [id_marca], [id_colecao], [dt_inclusao],
                             [timestamp], [fator_conversao], [codigo_integracao_ws], [codigo_integracao_reshop], [id_produtos_opticos_tipo], [id_sped_tipo_item], [componente], [altura_para_frete],
                             [largura_para_frete], [comprimento_para_frete], [loja_virtual], [codigoproduto_io], [cod_comprador], [altura], [largura], [comprimento], [codigo_integracao_oms]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cod_produto, @cod_barra, @nome, @ncm, @cest, @referencia, @cod_auxiliar, @unidade, @desc_cor, @desc_tamanho, 
                             @desc_setor, @desc_linha, @desc_marca, @desc_colecao, @dt_update, @cod_fornecedor, @desativado, @desc_espessura, @id_espessura, @desc_classificacao,
                             @id_classificacao, @origem_mercadoria, @peso_liquido, @peso_bruto, @id_cor, @id_tamanho, @id_setor, @id_linha, @id_marca, @id_colecao, @dt_inclusao,
                             @timestamp, @fator_conversao, @codigo_integracao_ws, @codigo_integracao_reshop, @id_produtos_opticos_tipo, @id_sped_tipo_item, @componente, @altura_para_frete,
                             @largura_para_frete, @comprimento_para_frete, @loja_virtual, @codigoproduto_io, @cod_comprador, @altura, @largura, @comprimento, @codigo_integracao_oms
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

        public async Task CallDbProcMergeAsync(string procName, string tableName, string database)
        {
            try
            {
                await _linxMicrovixRepositoryBase.CallDbProcMergeAsync(procName, tableName, database);
            }
            catch
            {
                throw;
            }
        }

        public void CallDbProcMergeNotAsync(string procName, string tableName, string database)
        {
            try
            {
                _linxMicrovixRepositoryBase.CallDbProcMergeNotAsync(procName, tableName, database);
            }
            catch
            {
                throw;
            }
        }
    }
}

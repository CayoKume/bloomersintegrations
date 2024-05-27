using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxProdutosDetalhesRepository : ILinxProdutosDetalhesRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxProdutosDetalhes> _linxMicrovixRepositoryBase;

        public LinxProdutosDetalhesRepository(ILinxMicrovixRepositoryBase<LinxProdutosDetalhes> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxProdutosDetalhes> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxProdutosDetalhes().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cnpj_emp, registros[i].cod_produto, registros[i].cod_barra,
                                   registros[i].quantidade, registros[i].preco_custo, registros[i].preco_venda, registros[i].custo_medio, registros[i].id_config_tributaria,
                                   registros[i].desc_config_tributaria, registros[i].despesas1, registros[i].qtde_minima, registros[i].qtde_maxima, registros[i].ipi,
                                   registros[i].timestamp, registros[i].custototal, registros[i].empresa);
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

        public async Task InsereRegistroIndividualAsync(LinxProdutosDetalhes registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [cod_produto], [cod_barra], [quantidade], [preco_custo], [preco_venda], [custo_medio],
                             [id_config_tributaria], [desc_config_tributaria], [despesas1], [qtde_minima], [qtde_maxima], [ipi], [timestamp], [custototal], 
                             [empresa]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @cod_produto, @cod_barra, @quantidade, @preco_custo, @preco_venda, @custo_medio,
                             @id_config_tributaria, @desc_config_tributaria, @despesas1, @qtde_minima, @qtde_maxima, @ipi, @timestamp, @custototal, 
                             @empresa
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

        public void InsereRegistroIndividualNotAsync(LinxProdutosDetalhes registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [cod_produto], [cod_barra], [quantidade], [preco_custo], [preco_venda], [custo_medio],
                             [id_config_tributaria], [desc_config_tributaria], [despesas1], [qtde_minima], [qtde_maxima], [ipi], [timestamp], [custototal], 
                             [empresa]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @cod_produto, @cod_barra, @quantidade, @preco_custo, @preco_venda, @custo_medio,
                             @id_config_tributaria, @desc_config_tributaria, @despesas1, @qtde_minima, @qtde_maxima, @ipi, @timestamp, @custototal, 
                             @empresa
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

        public async Task<List<LinxProdutosDetalhes>> GetRegistersExistsAsync(List<LinxProdutosDetalhes> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cod_produto}'";
                else
                    identificadores += $"'{registros[i].cod_produto}', ";
            }
            string query = $"SELECT cnpj_emp, cod_produto, timestamp FROM {database}.[dbo].{tableName} WHERE cod_produto IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxProdutosDetalhes> GetRegistersExistsNotAsync(List<LinxProdutosDetalhes> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cod_produto}'";
                else
                    identificadores += $"'{registros[i].cod_produto}', ";
            }
            string query = $"SELECT cnpj_emp, cod_produto, timestamp FROM {database}.[dbo].{tableName} WHERE cod_produto IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, query);
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

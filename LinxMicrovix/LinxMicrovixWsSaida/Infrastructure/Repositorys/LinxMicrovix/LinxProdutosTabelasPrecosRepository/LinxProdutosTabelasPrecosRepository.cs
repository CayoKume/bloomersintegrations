using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxProdutosTabelasPrecosRepository : ILinxProdutosTabelasPrecosRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxProdutosTabelasPrecos> _linxMicrovixRepositoryBase;

        public LinxProdutosTabelasPrecosRepository(ILinxMicrovixRepositoryBase<LinxProdutosTabelasPrecos> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxProdutosTabelasPrecos> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxProdutosTabelasPrecos().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cnpj_emp, registros[i].id_tabela, registros[i].cod_produto, registros[i].precovenda, registros[i].timestamp);
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

        public async Task InsereRegistroIndividualAsync(LinxProdutosTabelasPrecos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [id_tabela], [cod_produto], [precovenda], [timestamp]) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @id_tabela, @cod_produto, @precovenda, @timestamp)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(LinxProdutosTabelasPrecos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [id_tabela], [cod_produto], [precovenda], [timestamp]) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @id_tabela, @cod_produto, @precovenda, @timestamp)";

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

        public async Task<IEnumerable<String>> GetIdTabelaPrecoAsync(string cnpj, string tableName, string database)
        {
            string sql = $@"SELECT DISTINCT id_tabela FROM [BLOOMERS_LINX].[dbo].[LinxProdutosTabelas_trusted] (nolock) where cnpj_emp = '{cnpj}'";

            try
            {
                return await _linxMicrovixRepositoryBase.GetIdTabelaPrecoAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<string> GetIdTabelaPrecoNotAsync(string cnpj, string tableName, string database)
        {
            string sql = $@"SELECT DISTINCT id_tabela FROM [BLOOMERS_LINX].[dbo].[LinxProdutosTabelas_trusted] (nolock) where cnpj_emp = '{cnpj}'";

            try
            {
                return _linxMicrovixRepositoryBase.GetIdTabelaPrecoNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public Task<List<LinxProdutosTabelasPrecos>> GetRegistersExistsAsync(List<LinxProdutosTabelasPrecos> registros, string tableName, string database)
        {
            throw new NotImplementedException();
        }

        public List<LinxProdutosTabelasPrecos> GetRegistersExistsNotAsync(List<LinxProdutosTabelasPrecos> registros, string tableName, string database)
        {
            throw new NotImplementedException();
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

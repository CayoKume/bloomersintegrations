using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxProdutosInventarioRepository : ILinxProdutosInventarioRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxProdutosInventario> _linxMicrovixRepositoryBase;

        public LinxProdutosInventarioRepository(ILinxMicrovixRepositoryBase<LinxProdutosInventario> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxProdutosInventario> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxProdutosInventario().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cnpj_emp, registros[i].cod_produto, registros[i].cod_barra,
                                   registros[i].quantidade, registros[i].cod_deposito, registros[i].empresa);
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

        public async Task InsereRegistroIndividualAsync(LinxProdutosInventario registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [cod_produto], [cod_barra], [quantidade], [cod_deposito], [empresa]) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @cod_produto, @cod_barra, @quantidade, @cod_deposito, @empresa)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(LinxProdutosInventario registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [cod_produto], [cod_barra], [quantidade], [cod_deposito], [empresa]) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @cod_produto, @cod_barra, @quantidade, @cod_deposito, @empresa)";

            try
            {
                _linxMicrovixRepositoryBase.InsereRegistroIndividualNotAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<String>> GetCodDepositosAsync(string tableName)
        {
            string sql = $@"SELECT cod_deposito FROM [BLOOMERS_LINX].[dbo].[LinxProdutosDepositos_trusted] (nolock)";

            try
            {
                return await _linxMicrovixRepositoryBase.GetCodDepositosAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<string> GetCodDepositosNotAsync(string tableName)
        {
            string sql = $@"SELECT cod_deposito FROM [BLOOMERS_LINX].[dbo].[LinxProdutosDepositos_trusted] (nolock)";

            try
            {
                return _linxMicrovixRepositoryBase.GetCodDepositosNotAsync(tableName, sql);
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

        public async Task<List<LinxProdutosInventario>> GetRegistersExistsAsync(List<LinxProdutosInventario> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cod_produto}'";
                else
                    identificadores += $"'{registros[i].cod_produto}', ";
            }
            string query = $"SELECT cnpj_emp, cod_produto, lastupdateon FROM {database}.[dbo].{tableName} WHERE cod_produto IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxProdutosInventario> GetRegistersExistsNotAsync(List<LinxProdutosInventario> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cod_produto}'";
                else
                    identificadores += $"'{registros[i].cod_produto}', ";
            }
            string query = $"SELECT cnpj_emp, cod_produto, lastupdateon FROM {database}.[dbo].{tableName} WHERE cod_produto IN ({identificadores})";

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

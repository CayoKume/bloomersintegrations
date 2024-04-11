using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxProdutosDepositosRepository : ILinxProdutosDepositosRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxProdutosDepositos> _linxMicrovixRepositoryBase;

        public LinxProdutosDepositosRepository(ILinxMicrovixRepositoryBase<LinxProdutosDepositos> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxProdutosDepositos> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxProdutosDepositos().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cod_deposito, registros[i].nome_deposito, registros[i].disponivel,
                                   registros[i].disponivel_transferencia, registros[i].timestamp, registros[i].outlet);
                }

                _linxMicrovixRepositoryBase.BulkInsertIntoTableRaw(table, database, tableName, table.Rows.Count);
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

        public async Task<List<LinxProdutosDepositos>> GetRegistersExistsAsync(List<LinxProdutosDepositos> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cod_deposito}'";
                else
                    identificadores += $"'{registros[i].cod_deposito}', ";
            }
            string query = $"SELECT cod_deposito, timestamp FROM {database}.[dbo].{tableName} WHERE cod_deposito IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxProdutosDepositos> GetRegistersExistsNotAsync(List<LinxProdutosDepositos> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cod_deposito}'";
                else
                    identificadores += $"'{registros[i].cod_deposito}', ";
            }
            string query = $"SELECT cod_deposito, timestamp FROM {database}.[dbo].{tableName} WHERE cod_deposito IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public async Task InsereRegistroIndividualAsync(LinxProdutosDepositos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_deposito], [nome_deposito], [disponivel], [disponivel_transferencia], [timestamp], [outlet]) 
                            Values 
                            (@lastupdateon, @portal, @cod_deposito, @nome_deposito, @disponivel, @disponivel_transferencia, @timestamp, @outlet)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(LinxProdutosDepositos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_deposito], [nome_deposito], [disponivel], [disponivel_transferencia], [timestamp], [outlet]) 
                            Values 
                            (@lastupdateon, @portal, @cod_deposito, @nome_deposito, @disponivel, @disponivel_transferencia, @timestamp, @outlet)";

            try
            {
                _linxMicrovixRepositoryBase.InsereRegistroIndividualNotAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }
    }
}

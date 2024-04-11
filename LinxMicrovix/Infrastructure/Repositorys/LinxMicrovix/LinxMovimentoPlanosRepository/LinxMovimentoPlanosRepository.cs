using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Repositorys.Base;
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxMovimentoPlanosRepository : ILinxMovimentoPlanosRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxMovimentoPlanos> _linxMicrovixRepositoryBase;

        public LinxMovimentoPlanosRepository(ILinxMicrovixRepositoryBase<LinxMovimentoPlanos> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxMovimentoPlanos> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxMovimentoPlanos().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cnpj_emp, registros[i].identificador, registros[i].plano, registros[i].desc_plano, registros[i].total, registros[i].qtde_parcelas, registros[i].indice_plano,
                                   registros[i].cod_forma_pgto, registros[i].forma_pgto, registros[i].tipo_transacao, registros[i].taxa_financeira, registros[i].ordem_cartao, registros[i].timestamp, registros[i].empresa);
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

        public async Task<List<LinxMovimentoPlanos>> GetRegistersExistsAsync(List<LinxMovimentoPlanos> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj_emp}'";
                else
                    identificadores += $"'{registros[i].cnpj_emp}', ";
            }
            string query = $"SELECT cnpj_emp, identificador, TIMESTAMP FROM BLOOMERS_LINX..LinxMovimentoPlanos_trusted WHERE identificador IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxMovimentoPlanos> GetRegistersExistsNotAsync(List<LinxMovimentoPlanos> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj_emp}'";
                else
                    identificadores += $"'{registros[i].cnpj_emp}', ";
            }
            string query = $"SELECT cnpj_emp, identificador, TIMESTAMP FROM BLOOMERS_LINX..LinxMovimentoPlanos_trusted WHERE identificador IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public async Task InsereRegistroIndividualAsync(LinxMovimentoPlanos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [identificador], [plano], [desc_plano], [total], [qtde_parcelas],
                             [indice_plano], [cod_forma_pgto], [forma_pgto], [tipo_transacao], [taxa_financeira], [ordem_cartao],
                             [timestamp], [empresa]) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @identificador, @plano, @desc_plano, @total, @qtde_parcelas,
                             @indice_plano, @cod_forma_pgto, @forma_pgto, @tipo_transacao, @taxa_financeira, @ordem_cartao,
                             @timestamp, @empresa)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(LinxMovimentoPlanos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [identificador], [plano], [desc_plano], [total], [qtde_parcelas],
                             [indice_plano], [cod_forma_pgto], [forma_pgto], [tipo_transacao], [taxa_financeira], [ordem_cartao],
                             [timestamp], [empresa]) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @identificador, @plano, @desc_plano, @total, @qtde_parcelas,
                             @indice_plano, @cod_forma_pgto, @forma_pgto, @tipo_transacao, @taxa_financeira, @ordem_cartao,
                             @timestamp, @empresa)";

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

using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Repositorys.Base;
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxPlanosRepository : ILinxPlanosRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxPlanos> _linxMicrovixRepositoryBase;

        public LinxPlanosRepository(ILinxMicrovixRepositoryBase<LinxPlanos> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxPlanos> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxPlanos().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].plano, registros[i].desc_plano, registros[i].qtde_parcelas, registros[i].prazo_entre_parcelas, registros[i].tipo_plano, registros[i].indice_plano,
                                   registros[i].cod_forma_pgto, registros[i].forma_pgto, registros[i].conta_central, registros[i].tipo_transacao, registros[i].taxa_financeira, registros[i].dt_upd, registros[i].desativado, registros[i].usa_tef, 
                                   registros[i].timestamp);
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

        public async Task<List<LinxPlanos>> GetRegistersExistsAsync(List<LinxPlanos> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].plano}'";
                else
                    identificadores += $"'{registros[i].plano}', ";
            }
            string query = $"SELECT plano, timestamp FROM {database}.[dbo].{tableName} WHERE plano IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxPlanos> GetRegistersExistsNotAsync(List<LinxPlanos> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].plano}'";
                else
                    identificadores += $"'{registros[i].plano}', ";
            }
            string query = $"SELECT plano, timestamp FROM {database}.[dbo].{tableName} WHERE plano IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public async Task InsereRegistroIndividualAsync(LinxPlanos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [plano], [desc_plano], [qtde_parcelas], [prazo_entre_parcelas], [tipo_plano], [indice_plano],
                             [cod_forma_pgto], [forma_pgto], [conta_central], [tipo_transacao], [taxa_financeira], [dt_upd], [desativado],
                             [usa_tef], [timestamp]) 
                            Values 
                            (@lastupdateon, @portal, @plano, @desc_plano, @qtde_parcelas, @prazo_entre_parcelas, @tipo_plano, @indice_plano,
                             @cod_forma_pgto, @forma_pgto, @conta_central, @tipo_transacao, @taxa_financeira, @dt_upd, @desativado,
                             @usa_tef, @timestamp)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(LinxPlanos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [plano], [desc_plano], [qtde_parcelas], [prazo_entre_parcelas], [tipo_plano], [indice_plano],
                             [cod_forma_pgto], [forma_pgto], [conta_central], [tipo_transacao], [taxa_financeira], [dt_upd], [desativado],
                             [usa_tef], [timestamp]) 
                            Values 
                            (@lastupdateon, @portal, @plano, @desc_plano, @qtde_parcelas, @prazo_entre_parcelas, @tipo_plano, @indice_plano,
                             @cod_forma_pgto, @forma_pgto, @conta_central, @tipo_transacao, @taxa_financeira, @dt_upd, @desativado,
                             @usa_tef, @timestamp)";

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

using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Repositorys.Base;
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxGrupoLojasRepository : ILinxGrupoLojasRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxGrupoLojas> _linxMicrovixRepositoryBase;

        public LinxGrupoLojasRepository(ILinxMicrovixRepositoryBase<LinxGrupoLojas> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxGrupoLojas> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxGrupoLojas().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].cnpj, registros[i].nome_empresa, registros[i].id_empresas_rede, registros[i].rede, registros[i].portal, registros[i].nome_portal, registros[i].empresa, registros[i].lojas_proprias, registros[i].classificacao_portal);
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

        public async Task<List<LinxGrupoLojas>> GetRegistersExistsAsync(List<LinxGrupoLojas> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj}'";
                else
                    identificadores += $"'{registros[i].cnpj}', ";
            }
            string query = $"SELECT cnpj, lastupdateon FROM BLOOMERS_LINX..LinxGrupoLojas_trusted WHERE cnpj IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxGrupoLojas> GetRegistersExistsNotAsync(List<LinxGrupoLojas> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj}'";
                else
                    identificadores += $"'{registros[i].cnpj}', ";
            }
            string query = $"SELECT cnpj, lastupdateon FROM BLOOMERS_LINX..LinxGrupoLojas_trusted WHERE cnpj IN ({identificadores})";

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

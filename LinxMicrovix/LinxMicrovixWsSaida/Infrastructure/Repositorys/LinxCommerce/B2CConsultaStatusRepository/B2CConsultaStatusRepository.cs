using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce
{
    public class B2CConsultaStatusRepository : IB2CConsultaStatusRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaStatus> _linxMicrovixRepositoryBase;

        public B2CConsultaStatusRepository(ILinxMicrovixRepositoryBase<B2CConsultaStatus> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public void BulkInsertIntoTableRaw(List<B2CConsultaStatus> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new B2CConsultaStatus().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].id_status, registros[i].descricao_status, registros[i].timestamp, registros[i].portal);
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

        public async Task<List<B2CConsultaStatus>> GetRegistersExistsAsync(List<B2CConsultaStatus> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].id_status}'";
                else
                    identificadores += $"'{registros[i].id_status}', ";
            }
            string query = $"SELECT id_status, timestamp FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE id_status IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<B2CConsultaStatus> GetRegistersExistsNotAsync(List<B2CConsultaStatus> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].id_status}'";
                else
                    identificadores += $"'{registros[i].id_status}', ";
            }
            string query = $"SELECT id_status, timestamp FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE id_status IN ({identificadores})";

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

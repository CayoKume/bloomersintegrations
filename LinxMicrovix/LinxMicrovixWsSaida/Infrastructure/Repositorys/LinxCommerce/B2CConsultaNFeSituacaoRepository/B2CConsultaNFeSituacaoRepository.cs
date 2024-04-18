using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce
{
    public class B2CConsultaNFeSituacaoRepository : IB2CConsultaNFeSituacaoRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaNFeSituacao> _linxMicrovixRepositoryBase;

        public B2CConsultaNFeSituacaoRepository(ILinxMicrovixRepositoryBase<B2CConsultaNFeSituacao> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public void BulkInsertIntoTableRaw(List<B2CConsultaNFeSituacao> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new B2CConsultaNFeSituacao().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].id_nfe_situacao, registros[i].descricao, registros[i].timestamp, registros[i].portal);
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
            string sql = $@"SELECT {parameterCol} FROM [{database}].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

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
            string sql = $@"SELECT {parameterCol} FROM [{database}].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

            try
            {
                return _linxMicrovixRepositoryBase.GetParametersNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<B2CConsultaNFeSituacao>> GetRegistersExistsAsync(List<B2CConsultaNFeSituacao> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].id_nfe_situacao}'";
                else
                    identificadores += $"'{registros[i].id_nfe_situacao}', ";
            }
            string sql = $"SELECT id_nfe_situacao, timestamp FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE id_nfe_situacao IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public List<B2CConsultaNFeSituacao> GetRegistersExistsNotAsync(List<B2CConsultaNFeSituacao> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].id_nfe_situacao}'";
                else
                    identificadores += $"'{registros[i].id_nfe_situacao}', ";
            }
            string sql = $"SELECT id_nfe_situacao, timestamp FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE id_nfe_situacao IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }
    }
}

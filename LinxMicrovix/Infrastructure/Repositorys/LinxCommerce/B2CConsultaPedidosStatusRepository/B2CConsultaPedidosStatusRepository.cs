using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxCommerce
{
    public class B2CConsultaPedidosStatusRepository : IB2CConsultaPedidosStatusRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaPedidosStatus> _linxMicrovixRepositoryBase;

        public B2CConsultaPedidosStatusRepository(ILinxMicrovixRepositoryBase<B2CConsultaPedidosStatus> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public void BulkInsertIntoTableRaw(List<B2CConsultaPedidosStatus> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new B2CConsultaPedidosStatus().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].id, registros[i].id_status, registros[i].id_pedido, registros[i].data_hora, registros[i].anotacao, registros[i].timestamp, registros[i].portal);
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

        public async Task<List<B2CConsultaPedidosStatus>> GetRegistersExistsAsync(List<B2CConsultaPedidosStatus> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].id}'";
                else
                    identificadores += $"'{registros[i].id}', ";
            }
            string sql = $"SELECT id, timestamp FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE id IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public List<B2CConsultaPedidosStatus> GetRegistersExistsNotAsync(List<B2CConsultaPedidosStatus> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].id}'";
                else
                    identificadores += $"'{registros[i].id}', ";
            }
            string sql = $"SELECT id, timestamp FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE id IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task InsereRegistroIndividualAsync(B2CConsultaPedidosStatus registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [id], [id_status], [id_pedido], [data_hora], [anotacao], [timestamp], [portal]) 
                            Values 
                            (@lastupdateon, @id, @id_status, @id_pedido, @data_hora, @anotacao, @timestamp, @portal)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(B2CConsultaPedidosStatus registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [id], [id_status], [id_pedido], [data_hora], [anotacao], [timestamp], [portal]) 
                            Values 
                            (@lastupdateon, @id, @id_status, @id_pedido, @data_hora, @anotacao, @timestamp, @portal)";

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

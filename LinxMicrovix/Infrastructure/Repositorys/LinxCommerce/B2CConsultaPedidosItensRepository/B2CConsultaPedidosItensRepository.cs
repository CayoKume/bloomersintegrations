using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxCommerce
{
    public class B2CConsultaPedidosItensRepository : IB2CConsultaPedidosItensRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaPedidosItens> _linxMicrovixRepositoryBase;

        public B2CConsultaPedidosItensRepository(ILinxMicrovixRepositoryBase<B2CConsultaPedidosItens> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<B2CConsultaPedidosItens> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new B2CConsultaPedidosItens().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].id_pedido_item, registros[i].id_pedido, registros[i].codigoproduto, registros[i].quantidade, registros[i].vl_unitario, registros[i].timestamp, registros[i].portal);
                }

                _linxMicrovixRepositoryBase.BulkInsertIntoTableRaw(table, database, tableName, table.Rows.Count);
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetTableLastTimestampAsync(string database, string tableName)
        {
            string sql = $@"SELECT MIN(TIMESTAMP) 
                                FROM [BLOOMERS_LINX].[dbo].[B2CCONSULTAPEDIDOS_TRUSTED] A (nolock) 
                                WHERE 
                                --ID_PEDIDO IN ()
                                DT_PEDIDO > '{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}T00:00:00' 
                                AND DT_PEDIDO < '{DateTime.Today.ToString("yyyy-MM-dd")}T23:59:59'";

            try
            {
                return await _linxMicrovixRepositoryBase.GetTableLastTimestampAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public string GetTableLastTimestampNotAsync(string database, string tableName)
        {
            string sql = $@"SELECT MIN(TIMESTAMP) 
                                FROM [BLOOMERS_LINX].[dbo].[B2CCONSULTAPEDIDOS_TRUSTED] A (nolock) 
                                WHERE 
                                --ID_PEDIDO IN ()
                                DT_PEDIDO > '{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}T00:00:00' 
                                AND DT_PEDIDO < '{DateTime.Today.ToString("yyyy-MM-dd")}T23:59:59'";

            try
            {
                return _linxMicrovixRepositoryBase.GetTableLastTimestampNotAsync(tableName, sql);
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

        public async Task<List<B2CConsultaPedidosItens>> GetRegistersExistsAsync(List<B2CConsultaPedidosItens> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].id_pedido_item}'";
                else
                    identificadores += $"'{registros[i].id_pedido_item}', ";
            }
            string query = $"SELECT id_pedido_item, timestamp FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE id_pedido_item IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<B2CConsultaPedidosItens> GetRegistersExistsNotAsync(List<B2CConsultaPedidosItens> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].id_pedido_item}'";
                else
                    identificadores += $"'{registros[i].id_pedido_item}', ";
            }
            string query = $"SELECT id_pedido_item, timestamp FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE id_pedido_item IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public async Task InsereRegistroIndividualAsync(B2CConsultaPedidosItens registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [id_pedido_item], [id_pedido], [codigoproduto], [quantidade], [vl_unitario], [timestamp], [portal]) 
                            Values 
                            (@lastupdateon, @id_pedido_item, @id_pedido, @codigoproduto, @quantidade, @vl_unitario, @timestamp, @portal)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(B2CConsultaPedidosItens registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [id_pedido_item], [id_pedido], [codigoproduto], [quantidade], [vl_unitario], [timestamp], [portal]) 
                            Values 
                            (@lastupdateon, @id_pedido_item, @id_pedido, @codigoproduto, @quantidade, @vl_unitario, @timestamp, @portal)";

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

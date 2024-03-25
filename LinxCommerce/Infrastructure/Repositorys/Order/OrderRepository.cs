using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys.Base;
using Microsoft.Win32;
using System.Data;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ILinxCommerceRepositoryBase<Order> _linxCommerceRepositoryBase;

        public void BulkInsertIntoTableRaw(List<Order> registros, string? database, string? tableName)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetParameters(string tableName, string sql)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetRegistersExists(IEnumerable<string> ordersIds, string? tableName, string? database)
        {
            var orderIDs = String.Empty;
            for (int i = 0; i < ordersIds.Count(); i++)
            {
                if (i == ordersIds.Count() - 1)
                    orderIDs += $"'{ordersIds}'";
                else
                    orderIDs += $"'{ordersIds}', ";
            }

            string query = $"SELECT ORDERID, CUSTOMERID, GLOBALSTATUS, ORDERSTATUSID, PAYMENTSTATUS, SHIPMENTSTATUS FROM [{database}].[dbo].[{tableName}_TRUSTED] A (NOLOCK) WHERE ORDERID IN ({orderIDs})";

            try
            {
                var retorno = await _linxCommerceRepositoryBase.GetRegistersExists(tableName, query);
                return retorno.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

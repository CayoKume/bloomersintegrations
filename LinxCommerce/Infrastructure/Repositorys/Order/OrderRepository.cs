using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys.Base;
using Microsoft.Win32;
using System.Data;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ILinxCommerceRepositoryBase<Order> _linxCommerceRepositoryBase;

        public OrderRepository(ILinxCommerceRepositoryBase<Order> linxCommerceRepositoryBase) =>
            (_linxCommerceRepositoryBase) = (linxCommerceRepositoryBase);

        public void BulkInsertIntoTableRaw(List<Order> registros, string? database, string? tableName)
        {
            try
            {
                var orders = CreateDataTable(new List<string> { "AcquiredDate","CancelledDate","CommissionAmount","CreatedBy","CreatedChannel","CreatedDate","CustomerBirthDate","CustomerCNPJ","CustomerCPF","CustomerEmail","CustomerGender","CustomerID","CustomerName","CustomerPhone","CustomerType","DeliveryAmount","DeliveryPostalCode","DiscountAmount","Discounts","ExternalInfo","GlobalStatus","HasConflicts","ItemsCount","ItemsQty","MarketPlaceBrand","ModifiedBy","ModifiedDate","MultiSiteTenant","OrderGroupID","OrderGroupNumber","OrderID","OrderInvoice","OrderNumber","OrderStatusID","OrderType","OriginalOrderID","PaymentDate","PaymentStatus","PaymentTaxAmount","Properties","Remarks","SalesRepresentative","Seller","SellerCommissionAmount","ShipmentDate","ShipmentStatus","Shipments","ShopperTicketID","SubTotal","Tags","TaxAmount","Total","TotalDue","TotalPaid","TotalRefunded","TrafficSourceID","WebSiteID","WebSiteIntegrationID","WebSiteName","Wishlist","lastupdateon" });
                FillDataTable(orders, registros, new List<string> { "AcquiredDate", "CancelledDate", "CommissionAmount", "CreatedBy", "CreatedChannel", "CreatedDate", "CustomerBirthDate", "CustomerCNPJ", "CustomerCPF", "CustomerEmail", "CustomerGender", "CustomerID", "CustomerName", "CustomerPhone", "CustomerType", "DeliveryAmount", "DeliveryPostalCode", "DiscountAmount", "Discounts", "ExternalInfo", "GlobalStatus", "HasConflicts", "ItemsCount", "ItemsQty", "MarketPlaceBrand", "ModifiedBy", "ModifiedDate", "MultiSiteTenant", "OrderGroupID", "OrderGroupNumber", "OrderID", "OrderInvoice", "OrderNumber", "OrderStatusID", "OrderType", "OriginalOrderID", "PaymentDate", "PaymentStatus", "PaymentTaxAmount", "Properties", "Remarks", "SalesRepresentative", "Seller", "SellerCommissionAmount", "ShipmentDate", "ShipmentStatus", "Shipments", "ShopperTicketID", "SubTotal", "Tags", "TaxAmount", "Total", "TotalDue", "TotalPaid", "TotalRefunded", "TrafficSourceID", "WebSiteID", "WebSiteIntegrationID", "WebSiteName", "Wishlist", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(orders, database, tableName, orders.Rows.Count);


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<string> GetParameters(string tableName, string sql)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetRegistersExists(List<string> ordersIds, string? tableName, string? database)
        {
            var orderIDs = String.Empty;
            for (int i = 0; i < ordersIds.Count(); i++)
            {
                if (i == ordersIds.Count() - 1)
                    orderIDs += $"'{ordersIds[i]}'";
                else
                    orderIDs += $"'{ordersIds[i]}', ";
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

        private DataTable CreateDataTable(List<string> properties)
        {
            var dataTable = new DataTable();
            for (int i = 0; i < properties.Count(); i++) 
            {
                dataTable.Columns.Add(properties[i]);
            }
            return dataTable;
        }

        private static void FillDataTable(DataTable dataTable, List<Order> registros, List<string> properties)
        {
            try
            {
                for (int i = 0; i < registros.Count(); i++)
                {
                    DataRow row = dataTable.NewRow();

                    for (int j = 0; j < properties.Count(); j++)
                    {
                        if (properties[j] == "lastupdateon")
                            row[properties[j]] = DateTime.Now;

                        else if (properties[j] == "CustomerCPF")
                        {
                            var propertie = properties[j].Replace("Customer", "")[0].ToString().ToUpper() + properties[j].Replace("Customer", "").Substring(1).ToLower();
                            row[properties[j]] = registros[i].Customer.GetType().GetProperty(propertie).GetValue(registros[i].Customer) is not null ?
                            registros[i].Customer.GetType().GetProperty(propertie).GetValue(registros[i].Customer) : null;
                        }
                        else if (properties[j] == "CustomerPhone")
                        {
                            var propertie = "Cell" + properties[j].Replace("Customer", "");
                            row[properties[j]] = registros[i].Customer.Contact.GetType().GetProperty(propertie).GetValue(registros[i].Customer.Contact) is not null ?
                            registros[i].Customer.Contact.GetType().GetProperty(propertie).GetValue(registros[i].Customer.Contact) : null;
                        }
                        else if (properties[j] == "CustomerName" || properties[j] == "CustomerEmail" || properties[j] == "CustomerGender" || properties[j] == "CustomerBirthDate")
                        {
                            var propertie = properties[j].Replace("Customer", "");
                            row[properties[j]] = registros[i].Customer.GetType().GetProperty(propertie).GetValue(registros[i].Customer) is not null ?
                            registros[i].Customer.GetType().GetProperty(propertie).GetValue(registros[i].Customer) : null;
                        }
                        else
                            row[properties[j]] = registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) is not null ?
                            registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) : null;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

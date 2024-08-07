using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys.Base;
using System.Data;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ILinxCommerceRepositoryBase<Order> _linxCommerceRepositoryBase;

        public OrderRepository(ILinxCommerceRepositoryBase<Order> linxCommerceRepositoryBase) =>
            (_linxCommerceRepositoryBase) = (linxCommerceRepositoryBase);

        public void BulkInsertIntoTableRaw(List<Order> registros, string? database)
        {
            try
            {
                var orders = _linxCommerceRepositoryBase.CreateDataTable("Orders", new List<string> { "AcquiredDate","CancelledDate","CommissionAmount","CreatedBy","CreatedChannel","CreatedDate","CustomerBirthDate","CustomerCNPJ","CustomerCPF","CustomerEmail","CustomerGender","CustomerID","CustomerName","CustomerPhone","CustomerType","DeliveryAmount","DeliveryPostalCode","DiscountAmount","Discounts","ExternalInfo","GlobalStatus","HasConflicts","ItemsCount","ItemsQty","MarketPlaceBrand","ModifiedBy","ModifiedDate","MultiSiteTenant","OrderGroupID","OrderGroupNumber","OrderID","OrderInvoice","OrderNumber","OrderStatusID","OrderType","OriginalOrderID","PaymentDate","PaymentStatus","PaymentTaxAmount","Properties","Remarks","SalesRepresentative","Seller","SellerCommissionAmount","ShipmentDate","ShipmentStatus","Shipments","ShopperTicketID","SubTotal","Tags","TaxAmount","Total","TotalDue","TotalPaid","TotalRefunded","TrafficSourceID","WebSiteID","WebSiteIntegrationID","WebSiteName","Wishlist","lastupdateon" });
                FillDataTable(orders, registros, new List<string> { "AcquiredDate", "CancelledDate", "CommissionAmount", "CreatedBy", "CreatedChannel", "CreatedDate", "CustomerBirthDate", "CustomerCNPJ", "CustomerCPF", "CustomerEmail", "CustomerGender", "CustomerID", "CustomerName", "CustomerPhone", "CustomerType", "DeliveryAmount", "DeliveryPostalCode", "DiscountAmount", "Discounts", "ExternalInfo", "GlobalStatus", "HasConflicts", "ItemsCount", "ItemsQty", "MarketPlaceBrand", "ModifiedBy", "ModifiedDate", "MultiSiteTenant", "OrderGroupID", "OrderGroupNumber", "OrderID", "OrderInvoice", "OrderNumber", "OrderStatusID", "OrderType", "OriginalOrderID", "PaymentDate", "PaymentStatus", "PaymentTaxAmount", "Properties", "Remarks", "SalesRepresentative", "Seller", "SellerCommissionAmount", "ShipmentDate", "ShipmentStatus", "Shipments", "ShopperTicketID", "SubTotal", "Tags", "TaxAmount", "Total", "TotalDue", "TotalPaid", "TotalRefunded", "TrafficSourceID", "WebSiteID", "WebSiteIntegrationID", "WebSiteName", "Wishlist", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(orders, database, "Order", orders.Rows.Count);

                var orderItems = _linxCommerceRepositoryBase.CreateDataTable("OrderItems", new List<string> { "BundleKitDiscount","BundleKitDiscountValue","BundlePriceType","CatalogID","CatalogItemType","Depth","DiscountAmount","ExternalInfo","FormData","Height","InStockHandlingDays","IsDeleted","IsDeliverable","IsFreeOffer","IsFreeShipping","IsGiftWrapping","IsService","OrderID","OrderItemID","OutStockHandlingDays","ParentItemID","Price","PriceListID","ProductID","ProductIntegrationID","ProductName","Properties","Qty","SKU","SKUIntegrationID","SellerSKU","SkuID","SkuName","SpecialType","Status","Subtotal","TaxationAmount","Total","WareHouseID","WarehouseIntegrationID","WebSiteID","Weight","Width","lastupdateon" });
                FillDataTable(orderItems, registros, new List<string> { "BundleKitDiscount", "BundleKitDiscountValue", "BundlePriceType", "CatalogID", "CatalogItemType", "Depth", "DiscountAmount", "ExternalInfo", "FormData", "Height", "InStockHandlingDays", "IsDeleted", "IsDeliverable", "IsFreeOffer", "IsFreeShipping", "IsGiftWrapping", "IsService", "OrderID", "OrderItemID", "OutStockHandlingDays", "ParentItemID", "Price", "PriceListID", "ProductID", "ProductIntegrationID", "ProductName", "Properties", "Qty", "SKU", "SKUIntegrationID", "SellerSKU", "SkuID", "SkuName", "SpecialType", "Status", "Subtotal", "TaxationAmount", "Total", "WareHouseID", "WarehouseIntegrationID", "WebSiteID", "Weight", "Width", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(orderItems, database, "OrderItem", orderItems.Rows.Count);

                var orderAddress = _linxCommerceRepositoryBase.CreateDataTable("OrderAddress", new List<string> { "AddressLine","AddressNotes","AddressType","City","ContactDocumentNumber","ContactName","ContactPhone","Landmark","Name","Neighbourhood","Number","OrderAddressID","OrderID","PointOfSaleID","PostalCode","State","lastupdateon" });
                FillDataTable(orderAddress, registros, new List<string> { "AddressLine", "AddressNotes", "AddressType", "City", "ContactDocumentNumber", "ContactName", "ContactPhone", "Landmark", "Name", "Neighbourhood", "Number", "OrderAddressID", "OrderID", "PointOfSaleID", "PostalCode", "State", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(orderAddress, database, "OrderAddress", orderAddress.Rows.Count);

                var orderPaymentMethods = _linxCommerceRepositoryBase.CreateDataTable("OrderPaymentMethods", new List<string> { "AcquiredDate","Amount","CaptureDate","InstallmentAmount","Installments","IntegrationID","OrderID","OrderPaymentMethodID","PaidAmount","PaymentCancelledDate","PaymentDate","PaymentMethodID","PaymentNumber","Properties","ReconciliationNumber","RefundAmount","Status","TaxAmount","TransactionID","lastupdateon" });
                FillDataTable(orderPaymentMethods, registros, new List<string> { "AcquiredDate", "Amount", "CaptureDate", "InstallmentAmount", "Installments", "IntegrationID", "OrderID", "OrderPaymentMethodID", "PaidAmount", "PaymentCancelledDate", "PaymentDate", "PaymentMethodID", "PaymentNumber", "Properties", "ReconciliationNumber", "RefundAmount", "Status", "TaxAmount", "TransactionID", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(orderPaymentMethods, database, "OrderPaymentMethod", orderPaymentMethods.Rows.Count);

                var orderPaymentInfos = _linxCommerceRepositoryBase.CreateDataTable("OrderPaymentInfos", new List<string> { "OrderID","OrderPaymentMethodID","PaymentMethodID","TransactionID","Status","PaymentType","Alias","Title","Description","ImagePath","ProviderDocumentNumber","lastupdateon" });
                FillDataTable(orderPaymentInfos, registros, new List<string> { "OrderID", "OrderPaymentMethodID", "PaymentMethodID", "TransactionID", "Status", "PaymentType", "Alias", "Title", "Description", "ImagePath", "ProviderDocumentNumber", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(orderPaymentInfos, database, "OrderPayment", orderPaymentInfos.Rows.Count);

                var orderDeliveryMethods = _linxCommerceRepositoryBase.CreateDataTable("OrderDeliveryMethods", new List<string> { "Amount","CarrierName","DeliveryGroupID","DeliveryMethodAlias","DeliveryMethodID","DeliveryMethodType","DockID","ETA","ETADays","ExternalID","IntegrationID","LogisticContractId","LogisticContractName","LogisticOptionId","LogisticOptionName","OrderDeliveryMethodID","OrderID","PointOfSaleID","PointOfSaleIntegrationID","PointOfSaleName","ScheduleDate","ScheduleDisplayName","ScheduleEndTime","ScheduleShiftID","ScheduleStartTime","ScheduleTax","WarehouseID","WarehouseIntegrationID","lastupdateon" });
                FillDataTable(orderDeliveryMethods, registros, new List<string> { "Amount", "CarrierName", "DeliveryGroupID", "DeliveryMethodAlias", "DeliveryMethodID", "DeliveryMethodType", "DockID", "ETA", "ETADays", "ExternalID", "IntegrationID", "LogisticContractId", "LogisticContractName", "LogisticOptionId", "LogisticOptionName", "OrderDeliveryMethodID", "OrderID", "PointOfSaleID", "PointOfSaleIntegrationID", "PointOfSaleName", "ScheduleDate", "ScheduleDisplayName", "ScheduleEndTime", "ScheduleShiftID", "ScheduleStartTime", "ScheduleTax", "WarehouseID", "WarehouseIntegrationID", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(orderDeliveryMethods, database, "OrderDeliveryMethods", orderDeliveryMethods.Rows.Count);

                var orderShipments = _linxCommerceRepositoryBase.CreateDataTable("OrderShipments", new List<string> { "AssignUserId","AssignUserName","DeliveryMethodID","DockID","OrderID","OrderShipmentID","Packages","ShipmentNumber","ShipmentStatus","lastupdateon" });
                FillDataTable(orderShipments, registros, new List<string> { "AssignUserId", "AssignUserName", "DeliveryMethodID", "DockID", "OrderID", "OrderShipmentID", "Packages", "ShipmentNumber", "ShipmentStatus", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(orderShipments, database, "OrderShipments", orderShipments.Rows.Count);

                var orderPackages = _linxCommerceRepositoryBase.CreateDataTable("OrderPackages", new List<string> { "DeliveryMethodID","IsDeleted","Items","OrderPackageID","OrderShipmentID","PackageNumber","ShippedBy","ShippedDate","TrackingNumber","lastupdateon" });
                FillDataTable(orderPackages, registros, new List<string> { "DeliveryMethodID", "IsDeleted", "Items", "OrderPackageID", "OrderShipmentID", "PackageNumber", "ShippedBy", "ShippedDate", "TrackingNumber", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(orderPackages, database, "OrderPackages", orderPackages.Rows.Count);

                var orderSalesRepresentative = _linxCommerceRepositoryBase.CreateDataTable("OrderSalesRepresentative", new List<string> { "SalesRepresentativeID","Name","Email","ImageTimestamp","MaxDiscountAmount","CellPhone","DeliveryCommission","OrderCommission","FromPortfolio","Phone","IntegrationID","FriendlyCode","OrderID","OrderNumber","lastupdateon" });
                FillDataTable(orderSalesRepresentative, registros, new List<string> { "SalesRepresentativeID", "Name", "Email", "ImageTimestamp", "MaxDiscountAmount", "CellPhone", "DeliveryCommission", "OrderCommission", "FromPortfolio", "Phone", "IntegrationID", "FriendlyCode", "OrderID", "OrderNumber", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(orderSalesRepresentative, database, "SalesRepresentative", orderSalesRepresentative.Rows.Count);

                var orderCustomer = _linxCommerceRepositoryBase.CreateDataTable("OrderCustomer", new List<string> { "lastupdateon","CreatedDate","CustomerHash","CustomerID","CustomerStatusID","CustomerType","Email","Name","WebSiteID","BirthDate","Cpf","Gender","RG","Surname" });
                FillDataTable(orderCustomer, registros, new List<string> { "lastupdateon", "CreatedDate", "CustomerHash", "CustomerID", "CustomerStatusID", "CustomerType", "Email", "Name", "WebSiteID", "BirthDate", "Cpf", "Gender", "RG", "Surname" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(orderCustomer, database, "Person", orderCustomer.Rows.Count);
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> GetParameters(string tableName)
        {
            string query = $@"SELECT NUMBEROFDAYS FROM [BLOOMERS_LINX].[dbo].[LINXAPIPARAM] WHERE METHOD = '{tableName}'";

            try
            {
                return await _linxCommerceRepositoryBase.GetParameters(query);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Order>> GetRegistersExists(List<string> ordersIds, string? database)
        {
            var orderIDs = String.Empty;
            for (int i = 0; i < ordersIds.Count(); i++)
            {
                if (i == ordersIds.Count() - 1)
                    orderIDs += $"'{ordersIds[i]}'";
                else
                    orderIDs += $"'{ordersIds[i]}', ";
            }

            string query = @$"SELECT ORDERID, CUSTOMERID, GLOBALSTATUS, ORDERSTATUSID, PAYMENTSTATUS, SHIPMENTSTATUS 
                              FROM [{database}].[dbo].[ORDER_TRUSTED] (NOLOCK) WHERE ORDERID IN ({orderIDs})";

            try
            {
                var retorno = await _linxCommerceRepositoryBase.GetRegistersExists("ORDER_TRUSTED", query);
                return retorno.ToList();
            }
            catch
            {
                throw;
            }
        }

        private static void FillDataTable(DataTable dataTable, List<Order> registros, List<string> properties)
        {
            try
            {
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (dataTable.TableName == "Orders")
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

                            else if (properties[j] == "Discounts")
                                row[properties[j]] = registros[i].Discounts.Count() > 0 ? registros[i].Discounts.First().DiscountID : null;

                            else if (properties[j] == "OrderInvoice")
                                row[properties[j]] = registros[i].OrderInvoice is not null ? registros[i].OrderInvoice.OrderInvoiceID : null;

                            else if (properties[j] == "OrderType")
                                row[properties[j]] = registros[i].OrderType is not null ? registros[i].OrderType.OrderTypeID : null;

                            else if (properties[j] == "SalesRepresentative")
                                row[properties[j]] = registros[i].SalesRepresentative is not null ? registros[i].SalesRepresentative.SalesRepresentativeID : null;

                            else if (properties[j] == "Seller")
                                row[properties[j]] = registros[i].Seller is not null ? registros[i].Seller.SellerID : null;

                            else if (properties[j] == "Shipments")
                                row[properties[j]] = registros[i].Shipments.Count() > 0 ? registros[i].Shipments.First().OrderShipmentID : null;

                            else if (properties[j] == "Tags")
                                row[properties[j]] = registros[i].Tags.Count() > 0 ? registros[i].Tags.First().TagID : null;

                            else if (properties[j] == "MultiSiteTenant" || properties[j] == "Properties")
                                row[properties[j]] = null;

                            else
                                row[properties[j]] = registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) is not null ?
                                registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) : null;
                        }

                        dataTable.Rows.Add(row);
                    }
                    else if (dataTable.TableName == "OrderItems")
                    {
                        for (int k = 0; k < registros[i].Items.Count(); k++)
                        {
                            DataRow row = dataTable.NewRow();

                            for (int j = 0; j < properties.Count(); j++)
                            {
                                if (properties[j] == "lastupdateon")
                                    row[properties[j]] = DateTime.Now;

                                else if (properties[j] == "FormData" || properties[j] == "Properties")
                                    row[properties[j]] = null;

                                else
                                    row[properties[j]] = registros[i].Items[k].GetType().GetProperty(properties[j]).GetValue(registros[i].Items[k]) is not null ?
                                    registros[i].Items[k].GetType().GetProperty(properties[j]).GetValue(registros[i].Items[k]) : null;
                            }

                            dataTable.Rows.Add(row);
                        }
                    }
                    else if (dataTable.TableName == "OrderAddress")
                    {
                        for (int k = 0; k < registros[i].Addresses.Count(); k++)
                        {
                            DataRow row = dataTable.NewRow();

                            for (int j = 0; j < properties.Count(); j++)
                            {
                                if (properties[j] == "lastupdateon")
                                    row[properties[j]] = DateTime.Now;

                                else
                                    row[properties[j]] = registros[i].Addresses[k].GetType().GetProperty(properties[j]).GetValue(registros[i].Addresses[k]) is not null ?
                                    registros[i].Addresses[k].GetType().GetProperty(properties[j]).GetValue(registros[i].Addresses[k]) : null;
                            }

                            dataTable.Rows.Add(row);
                        }
                    }
                    else if (dataTable.TableName == "OrderPaymentMethods")
                    {
                        for (int k = 0; k < registros[i].PaymentMethods.Count(); k++)
                        {
                            DataRow row = dataTable.NewRow();

                            for (int j = 0; j < properties.Count(); j++)
                            {
                                if (properties[j] == "lastupdateon")
                                    row[properties[j]] = DateTime.Now;

                                else if (properties[j] == "Properties")
                                    row[properties[j]] = null;

                                else
                                    row[properties[j]] = registros[i].PaymentMethods[k].GetType().GetProperty(properties[j]).GetValue(registros[i].PaymentMethods[k]) is not null ?
                                    registros[i].PaymentMethods[k].GetType().GetProperty(properties[j]).GetValue(registros[i].PaymentMethods[k]) : null;
                            }

                            dataTable.Rows.Add(row);
                        }
                    }
                    else if (dataTable.TableName == "OrderPaymentInfos")
                    {
                        for (int k = 0; k < registros[i].PaymentMethods.Count(); k++)
                        {
                            DataRow row = dataTable.NewRow();

                            for (int j = 0; j < properties.Count(); j++)
                            {
                                if (properties[j] == "lastupdateon")
                                    row[properties[j]] = DateTime.Now;

                                else
                                    row[properties[j]] = registros[i].PaymentMethods[k].PaymentInfo.GetType().GetProperty(properties[j]).GetValue(registros[i].PaymentMethods[k].PaymentInfo) is not null ?
                                    registros[i].PaymentMethods[k].PaymentInfo.GetType().GetProperty(properties[j]).GetValue(registros[i].PaymentMethods[k].PaymentInfo) : null;
                            }

                            dataTable.Rows.Add(row);
                        }
                    }
                    else if (dataTable.TableName == "OrderDeliveryMethods")
                    {
                        for (int k = 0; k < registros[i].DeliveryMethods.Count(); k++)
                        {
                            DataRow row = dataTable.NewRow();

                            for (int j = 0; j < properties.Count(); j++)
                            {
                                if (properties[j] == "ScheduleEndTime" || properties[j] == "ScheduleStartTime")
                                    continue;

                                else if (properties[j] == "lastupdateon")
                                    row[properties[j]] = DateTime.Now;

                                else if (properties[j] == "OrderID")
                                    row[properties[j]] = registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) is not null ?
                                    registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) : null;

                                else
                                    row[properties[j]] = registros[i].DeliveryMethods[k].GetType().GetProperty(properties[j]).GetValue(registros[i].DeliveryMethods[k]) is not null ?
                                    registros[i].DeliveryMethods[k].GetType().GetProperty(properties[j]).GetValue(registros[i].DeliveryMethods[k]) : null;
                            }

                            dataTable.Rows.Add(row);
                        }
                    }
                    else if (dataTable.TableName == "OrderShipments")
                    {
                        for (int k = 0; k < registros[i].Shipments.Count(); k++)
                        {
                            DataRow row = dataTable.NewRow();

                            for (int j = 0; j < properties.Count(); j++)
                            {
                                if (properties[j] == "lastupdateon")
                                    row[properties[j]] = DateTime.Now;

                                else if (properties[j] == "Packages")
                                    row[properties[j]] = null;

                                else
                                    row[properties[j]] = registros[i].Shipments[k].GetType().GetProperty(properties[j]).GetValue(registros[i].Shipments[k]) is not null ?
                                    registros[i].Shipments[k].GetType().GetProperty(properties[j]).GetValue(registros[i].Shipments[k]) : null;
                            }

                            dataTable.Rows.Add(row);
                        }
                    }
                    else if (dataTable.TableName == "OrderPackages")
                    {
                        for (int k = 0; k < registros[i].Shipments.Count(); k++)
                        {
                            for (int l = 0; l < registros[i].Shipments[k].Packages.Count(); l++)
                            {
                                DataRow row = dataTable.NewRow();

                                for (int j = 0; j < properties.Count(); j++)
                                {
                                    if (properties[j] == "lastupdateon")
                                        row[properties[j]] = DateTime.Now;

                                    else if (properties[j] == "Items")
                                        row[properties[j]] = null;

                                    else
                                        row[properties[j]] = registros[i].Shipments[k].Packages[l].GetType().GetProperty(properties[j]).GetValue(registros[i].Shipments[k].Packages[l]) is not null ?
                                        registros[i].Shipments[k].Packages[l].GetType().GetProperty(properties[j]).GetValue(registros[i].Shipments[k].Packages[l]) : null;
                                }

                                dataTable.Rows.Add(row);
                            }
                        }
                    }
                    else if (dataTable.TableName == "OrderSalesRepresentative")
                    {
                        DataRow row = dataTable.NewRow();

                        for (int j = 0; j < properties.Count(); j++)
                        {
                            if (properties[j] == "lastupdateon")
                                row[properties[j]] = DateTime.Now;

                            else if (properties[j] == "OrderNumber")
                                row[properties[j]] = registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) is not null ?
                                registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) : null;

                            else if (properties[j] == "OrderID")
                                row[properties[j]] = registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) is not null ?
                                registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) : null;

                            else if (properties[j] == "DeliveryCommission" || properties[j] == "OrderCommission" || properties[j] == "FromPortfolio")
                            {
                                if (registros[i].SalesRepresentative.Commission is not null)
                                    row[properties[j]] = registros[i].SalesRepresentative.Commission.GetType().GetProperty(properties[j]).GetValue(registros[i].SalesRepresentative.Commission) is not null ?
                                    registros[i].SalesRepresentative.Commission.GetType().GetProperty(properties[j]).GetValue(registros[i].SalesRepresentative.Commission) : null;
                                else
                                    row[properties[j]] = null;
                            }

                            else
                                row[properties[j]] = registros[i].SalesRepresentative.GetType().GetProperty(properties[j]).GetValue(registros[i].SalesRepresentative) is not null ?
                                registros[i].SalesRepresentative.GetType().GetProperty(properties[j]).GetValue(registros[i].SalesRepresentative) : null;
                        }

                        dataTable.Rows.Add(row);
                    }
                    else if (dataTable.TableName == "OrderCustomer")
                    {
                        DataRow row = dataTable.NewRow();

                        for (int j = 0; j < properties.Count(); j++)
                        {
                            if (properties[j] == "lastupdateon")
                                row[properties[j]] = DateTime.Now;
                            else if (properties[j] == "Cpf")
                            {
                                var propertie = properties[j].Replace("Customer", "")[0].ToString().ToUpper() + properties[j].Replace("Customer", "").Substring(1).ToLower();
                                row[properties[j]] = registros[i].Customer.GetType().GetProperty(propertie).GetValue(registros[i].Customer) is not null ?
                                registros[i].Customer.GetType().GetProperty(propertie).GetValue(registros[i].Customer) : registros[i].Customer.GetType().GetProperty("Cnpj").GetValue(registros[i].Customer);
                            }
                            else
                                row[properties[j]] = registros[i].Customer.GetType().GetProperty(properties[j]).GetValue(registros[i].Customer) is not null ?
                                registros[i].Customer.GetType().GetProperty(properties[j]).GetValue(registros[i].Customer) : null;
                        }

                        dataTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{dataTable.TableName} - FillDataTable - Erro ao preencher datatable {dataTable.TableName} - {ex.Message}");
            }
        }
    }
}

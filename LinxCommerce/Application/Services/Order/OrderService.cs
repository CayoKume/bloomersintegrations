using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Enums;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Extensions;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Apis;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys;

namespace BloomersCommerceIntegrations.LinxCommerce.Application.Services
{
    public class OrderService<TEntity> : IOrderService<TEntity> where TEntity : SearchOrderResponse.Root, new()
    {
        private string CHAVE = LinxAPIAttributes.TypeEnum.chave.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authentication.ToName();
        private readonly IAPICall _apiCall;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository, IAPICall apiCall) =>
            (_orderRepository, _apiCall) = (orderRepository, apiCall);

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                //var days = Convert.ToInt32(await _linxOrderRepository.GetParameters("LinxEcomOrder", "numberofdays"));
                var days = 2;

                var objectRequest = new
                {
                    Page = new { PageIndex = 0, PageSize = 0 },
                    //Where = $"OrderNumber==\"MI-49055\"",
                    Where = $"(ModifiedDate>=\"{DateTime.Now.AddDays(-days).Date:yyyy-MM-dd}T00:00:00\" && ModifiedDate<=\"{DateTime.Now.Date:yyyy-MM-dd}T23:59:59\")",
                    WhereMetadata = "",
                    OrderBy = "",
                };

                var searchOrdersResponse = await _apiCall.PostRequest(objectRequest, "/v1/Sales/API.svc/web/SearchOrders", AUTENTIFICACAO, CHAVE);
                var searchOrders = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchOrderResponse.Root>(searchOrdersResponse);
                var ordersInSql = await _orderRepository.GetRegistersExists(searchOrders.Result.Select(r => r.OrderID).ToList(), tableName, database);

                for (int i = 0; i < ordersInSql.Count(); i++)
                {
                    var order = searchOrders.Result.Where(r => r.OrderID == ordersInSql[i].OrderID).First();
                    if (order.CustomerID == ordersInSql[i].CustomerID && order.GlobalStatus == ordersInSql[i].GlobalStatus &&
                        order.OrderStatusID == ordersInSql[i].OrderStatusID && order.PaymentStatus == ordersInSql[i].PaymentStatus &&
                        order.ShipmentStatus == ordersInSql[i].ShipmentStatus)
                    {
                        searchOrders.Result.Remove(searchOrders.Result.Where(r => r.OrderID == ordersInSql[i].OrderID).First());
                    }
                    else
                        continue;
                }

                if (searchOrders.Result.Count() > 0)
                {
                    var ordersListToAdd = new List<Order>();

                    foreach (var registro in searchOrders.Result)
                    {
                        var orderResponse = await _apiCall.PostRequest(registro.OrderID, "/v1/LinxIO/API.svc/web/GetOrder", AUTENTIFICACAO, CHAVE);
                        var orderPaymentResponse = await _apiCall.PostRequest(registro.OrderID, "/v1/Sales/API.svc/web/GetOrderPayments", AUTENTIFICACAO, CHAVE);
                        var orderCustomerResponse = await _apiCall.PostRequest(registro.CustomerID, "/v1/Profile/API.svc/web/GetPerson", AUTENTIFICACAO, CHAVE);

                        var order = Newtonsoft.Json.JsonConvert.DeserializeObject<GetOrderResponse.Root>(orderResponse);
                        var orderPayment = Newtonsoft.Json.JsonConvert.DeserializeObject<GetOrderPaymentsResponse.Root>(orderPaymentResponse);
                        var orderClient = Newtonsoft.Json.JsonConvert.DeserializeObject<GetPersonResponse.Root>(orderCustomerResponse);

                        for (int i = 0; i < order.PaymentMethods.Count(); i++)
                        {
                            for (int j = 0; j < orderPayment.Result.Count(); j++)
                            {
                                order.PaymentMethods[i].PaymentInfo.Alias = orderPayment.Result[j].Alias;
                                order.PaymentMethods[i].PaymentInfo.Description = orderPayment.Result[j].Description;
                                order.PaymentMethods[i].PaymentInfo.ImagePath = orderPayment.Result[j].ImagePath;
                                order.PaymentMethods[i].PaymentInfo.OrderID = orderPayment.Result[j].OrderID;
                                order.PaymentMethods[i].PaymentInfo.OrderPaymentMethodID = orderPayment.Result[j].OrderPaymentMethodID;
                                order.PaymentMethods[i].PaymentInfo.PaymentMethodID = orderPayment.Result[j].PaymentMethodID;
                                order.PaymentMethods[i].PaymentInfo.PaymentType = orderPayment.Result[j].PaymentType;
                                order.PaymentMethods[i].PaymentInfo.ProviderDocumentNumber = orderPayment.Result[j].ProviderDocumentNumbe;
                                order.PaymentMethods[i].PaymentInfo.Status = orderPayment.Result[j].Status;
                                order.PaymentMethods[i].PaymentInfo.Title = orderPayment.Result[j].Title;
                                order.PaymentMethods[i].PaymentInfo.TransactionID = orderPayment.Result[j].TransactionID;
                            }
                        }

                        for (int i = 0; i < order.Shipments.Count(); i++)
                        {
                            if (searchOrders.Result.Where(r => r.OrderID == registro.OrderID).First()
                                .Fulfillments.Where(f => f.OrderID == registro.OrderID).Count() > 0)
                            {
                                order.Shipments[i].Packages =
                                searchOrders.Result.Where(r => r.OrderID == registro.OrderID).First()
                                .Fulfillments.Where(f => f.OrderID == registro.OrderID).First().Shipment.Packages;
                            }
                            else
                                order.Shipments[i].Packages = new List<Package>();
                        }

                        ordersListToAdd.Add(new Order
                        {
                            AcquiredDate = searchOrders.Result.Where(r => r.OrderID == order.OrderID).First().AcquiredDate,
                            CancelledDate = order.CancelledDate,
                            CommissionAmount = order.CommissionAmount,
                            CreatedBy = order.CreatedBy,
                            CreatedChannel = order.CreatedChannel,
                            CreatedDate = order.CreatedDate,
                            CustomerCNPJ = order.CustomerCNPJ,
                            CustomerID = order.CustomerID,
                            CustomerType = order.CustomerType,
                            DeliveryAmount = order.DeliveryAmount,
                            DeliveryPostalCode = order.DeliveryPostalCode,
                            DiscountAmount = order.DiscountAmount,
                            GlobalStatus = order.GlobalStatus,
                            HasConflicts = order.HasConflicts,
                            ItemsCount = order.ItemsCount,
                            ItemsQty = order.ItemsQty,
                            MarketPlaceBrand = order.MarketPlaceBrand,
                            ModifiedBy = order.ModifiedBy,
                            ModifiedDate = order.ModifiedDate,
                            OrderGroupID = order.OrderGroupID,
                            OrderGroupNumber = order.OrderGroupNumber,
                            OrderID = order.OrderID,
                            OrderNumber = order.OrderNumber,
                            OrderStatusID = order.OrderStatusID,
                            OriginalOrderID = order.OriginalOrderID,
                            PaymentDate = order.PaymentDate,
                            PaymentStatus = order.PaymentStatus,
                            PaymentTaxAmount = order.PaymentTaxAmount,
                            Remarks = order.Remarks,
                            SellerCommissionAmount = order.SellerCommissionAmount,
                            ShipmentDate = order.ShipmentDate,
                            ShipmentStatus = order.ShipmentStatus,
                            ShopperTicketID = order.ShopperTicketID,
                            SubTotal = order.SubTotal,
                            TaxAmount = order.TaxAmount,
                            Total = order.Total,
                            TotalDue = order.TotalDue,
                            TotalPaid = order.TotalPaid,
                            TotalRefunded = order.TotalRefunded,
                            TrafficSourceID = order.TrafficSourceID,
                            WebSiteID = order.WebSiteID,
                            WebSiteIntegrationID = order.WebSiteIntegrationID,
                            WebSiteName = order.WebSiteName,

                            DeliveryMethods = order.DeliveryMethods,
                            Addresses = order.Addresses,
                            Items = order.Items,
                            Discounts = order.Discounts,
                            ExternalInfo = order.ExternalInfo,
                            OrderInvoice = order.OrderInvoice,
                            SalesRepresentative = order.SalesRepresentative,
                            Seller = order.Seller,
                            Shipments = order.Shipments,
                            Tags = order.Tags,
                            Wishlist = order.Wishlist,
                            PaymentMethods = order.PaymentMethods,
                            Customer = orderClient
                        });
                    }

                    _orderRepository.BulkInsertIntoTableRaw(ordersListToAdd, database, tableName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void IntegraRegistrosSync(string tableName, string procName, string database)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador)
        {
            throw new NotImplementedException();
        }

        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador)
        {
            throw new NotImplementedException();
        }

        public List<Order>? T1ToObject(TEntity entity)
        {
            var list = new List<Order>();
            for (int i = 0; i < entity.Result.Count(); i++)
            {
                list.Add(new Order
                {
                    //OrderID = entity.Result[i].OrderID,
                    //OrderNumber = entity.Result[i].OrderNumber,
                    //MarketPlaceBrand = entity.Result[i].MarketPlaceBrand,
                    //OriginalOrderID = entity.Result[i].OriginalOrderID,
                    //WebSiteID = entity.Result[i].WebSiteID,
                    //WebSiteIntegrationID = entity.Result[i].WebSiteIntegrationID,
                    //CustomerID = entity.Result[i].CustomerID,
                    //ShopperTicketID = entity.Result[i].ShopperTicketID,
                    //ItemsQty = entity.Result[i].ItemsQty,
                    //ItemsCount = entity.Result[i].ItemsCount,
                    //TaxAmount = entity.Result[i].TaxAmount,
                    //DeliveryAmount = entity.Result[i].DeliveryAmount,
                    //DiscountAmount = entity.Result[i].DiscountAmount,
                    //PaymentTaxAmount = entity.Result[i].PaymentTaxAmount,
                    //SubTotal = entity.Result[i].SubTotal,
                    //Total = entity.Result[i].Total,
                    //TotalDue = entity.Result[i].TotalDue,
                    //TotalPaid = entity.Result[i].TotalPaid,
                    //TotalRefunded = entity.Result[i].TotalRefunded,
                    //PaymentDate = entity.Result[i].PaymentDate,
                    //PaymentStatus = entity.Result[i].PaymentStatus,
                    //ShipmentDate = entity.Result[i].ShipmentDate,
                    //ShipmentStatus = entity.Result[i].ShipmentStatus,
                    //GlobalStatus = entity.Result[i].GlobalStatus,
                    //DeliveryPostalCode = entity.Result[i].DeliveryPostalCode,
                    //CreatedChannel = entity.Result[i].CreatedChannel,
                    //TrafficSourceID = entity.Result[i].TrafficSourceID,
                    //OrderStatusID = entity.Result[i].OrderStatusID,
                    //Items = new Item
                    //{

                    //}
                    //Tags = entity.Result[i].
                    //Properties = entity.Result[i].
                    //Addresses = entity.Result[i].
                    //PaymentMethods = entity.Result[i].
                    //DeliveryMethods = entity.Result[i].
                    //Discounts = entity.Result[i].
                    //Shipments = entity.Result[i].
                    //Fulfillments = entity.Result[i].
                    //CreatedDate = entity.Result[i].
                    //CreatedBy = entity.Result[i].
                    //ModifiedDate = entity.Result[i].
                    //ModifiedBy = entity.Result[i].
                    //Remarks = entity.Result[i].
                    //Seller = entity.Result[i].
                    //SellerCommissionAmount = entity.Result[i].
                    //SalesRepresentative = entity.Result[i].
                    //CommissionAmount = entity.Result[i].
                    //OrderType = entity.Result[i].
                    //OrderInvoice = entity.Result[i].
                    //OrderGroupID = entity.Result[i].
                    //OrderGroupNumber = entity.Result[i].
                    //HasConflicts = entity.Result[i].
                    //AcquiredDate = entity.Result[i].
                    //ExternalInfo = entity.Result[i].
                    //HasHubOrderWithoutShipmentConflict = entity.Result[i].
                    //CustomerType = entity.Result[i].
                    //MultiSiteTenant = entity.Result[i].
                    //CancelledDate = entity.Result[i].
                    //Wishlist = entity.Result[i].
                    //WebSiteName = entity.Result[i].
                    //CustomerName = entity.Result[i].
                    //CustomerEmail = entity.Result[i].
                    //CustomerGender = entity.Result[i].
                    //CustomerBirthDate = entity.Result[i].
                    //CustomerPhone = entity.Result[i].
                    //CustomerCPF = entity.Result[i].
                    //CustomerCNPJ = entity.Result[i].
                    //CustomerTradingName = entity.Result[i].
                    //CustomerSiteTaxPayer = entity.Result[i].
                });
            }
            return list;
        }
    }
}

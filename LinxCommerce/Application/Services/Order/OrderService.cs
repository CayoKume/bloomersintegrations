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

                var response = await _apiCall.PostRequest(objectRequest, "/v1/Sales/API.svc/web/SearchOrders", AUTENTIFICACAO, CHAVE);
                var registros = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchOrderResponse.Root>(response);
                var registrosInSql = await _orderRepository.GetRegistersExists(registros.Result.Select(r => r.OrderID), tableName, database);

                for (int i = 0; i < registrosInSql.Count(); i++)
                {
                    var registro = registros.Result.Where(r => r.OrderID == registrosInSql[i].OrderID).First();
                    if (registro.CustomerID == registrosInSql[i].CustomerID && registro.GlobalStatus == registrosInSql[i].GlobalStatus &&
                        registro.OrderStatusID == registrosInSql[i].OrderStatusID && registro.PaymentStatus == registrosInSql[i].PaymentStatus &&
                        registro.ShipmentStatus == registrosInSql[i].ShipmentStatus)
                    {
                        registros.Result.Remove(registros.Result.Where(r => r.OrderID == registrosInSql[i].OrderID).First());
                    }
                    else
                        continue;
                }

                if (registros.Result.Count() > 0)
                {
                    var register = new SearchOrderResponse.Root { Result = new List<SearchOrderResponse.Result>() };

                    foreach (var registro in registros.Result)
                    {
                        var _response = await _apiCall.PostRequest(registro.OrderID, "/v1/LinxIO/API.svc/web/GetOrder", AUTENTIFICACAO, CHAVE);
                        var __response = await _apiCall.PostRequest(registro.OrderID, "/v1/Sales/API.svc/web/GetOrderPayments", AUTENTIFICACAO, CHAVE);

                        var _registro = Newtonsoft.Json.JsonConvert.DeserializeObject<GetOrderResponse.Root>(_response);
                        var __registro = Newtonsoft.Json.JsonConvert.DeserializeObject<GetOrderPaymentsResponse.Root>(__response);

                        for (int i = 0; i < _registro.PaymentMethods.Count(); i++)
                        {
                            for (int j = 0; j < __registro.Result.Count(); j++)
                            {
                                _registro.PaymentMethods[i].PaymentInfo.Alias = __registro.Result[j].Alias;
                                _registro.PaymentMethods[i].PaymentInfo.Description = __registro.Result[j].Description;
                                _registro.PaymentMethods[i].PaymentInfo.ImagePath = __registro.Result[j].ImagePath;
                                _registro.PaymentMethods[i].PaymentInfo.OrderID = __registro.Result[j].OrderID;
                                _registro.PaymentMethods[i].PaymentInfo.OrderPaymentMethodID = __registro.Result[j].OrderPaymentMethodID;
                                _registro.PaymentMethods[i].PaymentInfo.PaymentMethodID = __registro.Result[j].PaymentMethodID;
                                _registro.PaymentMethods[i].PaymentInfo.PaymentType = __registro.Result[j].PaymentType;
                                _registro.PaymentMethods[i].PaymentInfo.ProviderDocumentNumber = __registro.Result[j].ProviderDocumentNumbe;
                                _registro.PaymentMethods[i].PaymentInfo.Status = __registro.Result[j].Status;
                                _registro.PaymentMethods[i].PaymentInfo.Title = __registro.Result[j].Title;
                                _registro.PaymentMethods[i].PaymentInfo.TransactionID = __registro.Result[j].TransactionID;
                            }
                        }

                        for (int i = 0; i < _registro.Shipments.Count(); i++)
                        {
                            if (registros.Result.Where(r => r.OrderID == registro.OrderID).First()
                                .Fulfillments.Where(f => f.OrderID == registro.OrderID).Count() > 0)
                            {
                                _registro.Shipments[i].Packages =
                                registros.Result.Where(r => r.OrderID == registro.OrderID).First()
                                .Fulfillments.Where(f => f.OrderID == registro.OrderID).First().Shipment.Packages;
                            }
                            else
                                _registro.Shipments[i].Packages = new List<Package>();
                        }

                        register.Result.Add(new SearchOrderResponse.Result
                        {
                            AcquiredDate = registros.Result.Where(r => r.OrderID == _registro.OrderID).First().AcquiredDate,
                            CancelledDate = _registro.CancelledDate,
                            CommissionAmount = _registro.CommissionAmount,
                            CreatedBy = _registro.CreatedBy,
                            CreatedChannel = _registro.CreatedChannel,
                            CreatedDate = _registro.CreatedDate,
                            CustomerBirthDate = _registro.CustomerBirthDate,
                            CustomerCNPJ = _registro.CustomerCNPJ,
                            CustomerCPF = _registro.CustomerCPF,
                            CustomerEmail = _registro.CustomerEmail,
                            CustomerGender = _registro.CustomerGender,
                            CustomerID = _registro.CustomerID,
                            CustomerName = _registro.CustomerName,
                            CustomerPhone = _registro.CustomerPhone,
                            CustomerType = _registro.CustomerType,
                            DeliveryAmount = _registro.DeliveryAmount,
                            DeliveryPostalCode = _registro.DeliveryPostalCode,
                            DiscountAmount = _registro.DiscountAmount,
                            GlobalStatus = _registro.GlobalStatus,
                            HasConflicts = _registro.HasConflicts,
                            ItemsCount = _registro.ItemsCount,
                            ItemsQty = _registro.ItemsQty,
                            MarketPlaceBrand = _registro.MarketPlaceBrand,
                            ModifiedBy = _registro.ModifiedBy,
                            ModifiedDate = _registro.ModifiedDate,
                            OrderGroupID = _registro.OrderGroupID,
                            OrderGroupNumber = _registro.OrderGroupNumber,
                            OrderID = _registro.OrderID,
                            OrderNumber = _registro.OrderNumber,
                            OrderStatusID = _registro.OrderStatusID,
                            OriginalOrderID = _registro.OriginalOrderID,
                            PaymentDate = _registro.PaymentDate,
                            PaymentStatus = _registro.PaymentStatus,
                            PaymentTaxAmount = _registro.PaymentTaxAmount,
                            Remarks = _registro.Remarks,
                            SellerCommissionAmount = _registro.SellerCommissionAmount,
                            ShipmentDate = _registro.ShipmentDate,
                            ShipmentStatus = _registro.ShipmentStatus,
                            ShopperTicketID = _registro.ShopperTicketID,
                            SubTotal = _registro.SubTotal,
                            TaxAmount = _registro.TaxAmount,
                            Total = _registro.Total,
                            TotalDue = _registro.TotalDue,
                            TotalPaid = _registro.TotalPaid,
                            TotalRefunded = _registro.TotalRefunded,
                            TrafficSourceID = _registro.TrafficSourceID,
                            WebSiteID = _registro.WebSiteID,
                            WebSiteIntegrationID = _registro.WebSiteIntegrationID,
                            WebSiteName = _registro.WebSiteName,

                            DeliveryMethods = _registro.DeliveryMethods,
                            Addresses = _registro.Addresses,
                            Items = _registro.Items,
                            Discounts = _registro.Discounts,
                            ExternalInfo = _registro.ExternalInfo,
                            OrderInvoice = _registro.OrderInvoice,
                            SalesRepresentative = _registro.SalesRepresentative,
                            Seller = _registro.Seller,
                            Shipments = _registro.Shipments,
                            Tags = _registro.Tags,
                            Wishlist = _registro.Wishlist,
                            PaymentMethods = _registro.PaymentMethods,
                        });
                    }
                    //_orderRepository.BulkInsertIntoTableRaw(register, tableName, database);
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

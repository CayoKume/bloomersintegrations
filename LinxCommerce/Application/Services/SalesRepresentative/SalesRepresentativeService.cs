using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Enums;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Extensions;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Apis;

namespace BloomersCommerceIntegrations.LinxCommerce.Application.Services
{
    public class SalesRepresentativeService : ISalesRepresentativeService
    {
        private string CHAVE = LinxAPIAttributes.TypeEnum.chave.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authentication.ToName();
        private readonly IAPICall _apiCall;

        public SalesRepresentativeService(IAPICall apiCall) =>
            (_apiCall) = (apiCall);

        public async Task<bool> AlteraRegistros()
        {
            try
            {
                var objectSearchSalesRepresentativeRequest = new
                {
                    Page = new { PageIndex = 0, PageSize = 0 },
                    WhereMetadata = "",
                    OrderBy = "",
                };

                var searchSalesRepresentativeResponse = await _apiCall.PostRequest(objectSearchSalesRepresentativeRequest, "/v1/Sales/API.svc/web/SearchSalesRepresentative", AUTENTIFICACAO, CHAVE);
                var searchSalesRepresentative = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchSalesRepresentativeResponse.Root>(searchSalesRepresentativeResponse);

                foreach (var registroSearchSalesRepresentative in searchSalesRepresentative.Result)
                {
                    var objectGetSalesRepresentativeRequest = new
                    {
                        SalesRepresentativeID = registroSearchSalesRepresentative.SalesRepresentativeID
                    };

                    var registroGetSalesRepresentativeResponse = await _apiCall.PostRequest(objectGetSalesRepresentativeRequest, "/v1/Sales/API.svc/web/GetSalesRepresentative", AUTENTIFICACAO, CHAVE);
                    var getSalesRepresentative = Newtonsoft.Json.JsonConvert.DeserializeObject<BloomersCommerceIntegrations.LinxCommerce.Domain.Entities.GetSalesRepresentativeResponse.GetSalesRepresentativeResponse.Root>(registroGetSalesRepresentativeResponse);

                    var jObject = new
                    {
                        SalesRepresentativeID = getSalesRepresentative.SalesRepresentative.SalesRepresentativeID,
                        RemovePhoto = true,
                        RemoveNotInformedAddresses = true,
                        RemoveNotInformedCustomers = true,
                        RemoveNotInformedUsers = true,
                        Portfolio = new { HasPortfolio = getSalesRepresentative.SalesRepresentative.Portfolio.HasPortfolio, PortfolioAssociationType = "D", Customers = new List<SalesRepresentativeCustomerRelation>() },
                        ShippingRegion = getSalesRepresentative.SalesRepresentative.ShippingRegion,
                        WebSiteSettings = getSalesRepresentative.SalesRepresentative.WebSiteSettings,
                        Status = getSalesRepresentative.SalesRepresentative.Status,
                        SalesRepresentativeType = getSalesRepresentative.SalesRepresentative.SalesRepresentativeType,
                        Name = getSalesRepresentative.SalesRepresentative.Name,
                        Identification = getSalesRepresentative.SalesRepresentative.Identification,
                        FriendlyCode = getSalesRepresentative.SalesRepresentative.FriendlyCode,
                        Contact = getSalesRepresentative.SalesRepresentative.Contact,
                        OrderTypeItems = getSalesRepresentative.SalesRepresentative.OrderTypeItems,
                        AllowQuoteDeletion = getSalesRepresentative.SalesRepresentative.AllowQuoteDeletion,
                        MaxDiscount = getSalesRepresentative.SalesRepresentative.MaxDiscount,
                        PortfolioCommission = getSalesRepresentative.SalesRepresentative.PortfolioCommission,
                        GeneralCommission = getSalesRepresentative.SalesRepresentative.GeneralCommission
                    };

                    var saveSalesRepresentativeResponse = await _apiCall.PostRequest(jObject, "/v1/Sales/API.svc/web/SaveSalesRepresentative", AUTENTIFICACAO, CHAVE);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

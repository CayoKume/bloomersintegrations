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
                var objectRequest = new
                {
                    Page = new { PageIndex = 0, PageSize = 0 },
                    WhereMetadata = "",
                    OrderBy = "",
                };

                var searchSalesRepresentativeResponse = await _apiCall.PostRequest(objectRequest, "/v1/Sales/API.svc/web/SearchSalesRepresentative", AUTENTIFICACAO, CHAVE);
                var searchSalesRepresentatives = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchSalesRepresentativeResponse.Root>(searchSalesRepresentativeResponse);

                foreach (var registro in searchSalesRepresentatives.Result)
                {
                    var jObject = new
                    {
                        SalesRepresentativeID = registro.SalesRepresentativeID,
                        RemovePhoto = true,
                        RemoveNotInformedAddresses = true,
                        RemoveNotInformedCustomers = true,
                        RemoveNotInformedUsers = true,
                        Portfolio = new { HasPortfolio = registro.HasPortfolio, PortfolioAssociationType = "D", Customers = new List<SalesRepresentativeCustomerRelation>() },
                        ShippingRegion = new { SelectedMode = registro.ShippingRegionSelectedMode },
                        WebSiteSettings = new { WebSiteFilter = "A" },
                        Status = registro.Status,
                        SalesRepresentativeType = registro.SalesRepresentativeType,
                        Name = registro.Name,
                        Identification = registro.Identification,
                        FriendlyCode = registro.FriendlyCode,
                        Contact = registro.Contact,
                        OrderTypeItems = registro.OrderTypeItems,
                        AllowQuoteDeletion = registro.AllowQuoteDeletion,
                        MaxDiscount = registro.MaxDiscount,
                        PortfolioCommission = registro.PortfolioCommission,
                        GeneralCommission = registro.GeneralCommission
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

namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities.GetSalesRepresentativeResponse
{
    public class GetSalesRepresentativeResponse
    {
        public class Root
        {
            public SalesRepresentative SalesRepresentative { get; set; }
        }

        public class SalesRepresentative
        {
            public string ImageUrl { get; set; }
            public SalesRepresentativePortfolio Portfolio { get; set; }
            public SalesRepresentativeWebSiteSettings ShippingRegion { get; set; }
            public SalesRepresentativeWebSiteSettings WebSiteSettings { get; set; }
            public List<SalesRepresentativeAddress> Addresses { get; set; }
            public List<int> UserIDs { get; set; }
            public int SalesRepresentativeID { get; set; }
            public string Status { get; set; }
            public string SalesRepresentativeType { get; set; }
            public string Name { get; set; }
            public SalesRepresentativeIdentification Identification { get; set; }
            public string FriendlyCode { get; set; }
            public string IntegrationID { get; set; }
            public SalesRepresentativeContactData Contact { get; set; }
            public List<int> OrderTypeItems { get; set; }
            public List<int> CompetenceApproversList { get; set; }
            public bool AllowQuoteDeletion { get; set; }
            public int? BusinessContractID { get; set; }
            public SalesRepresentativeMaxDiscount MaxDiscount { get; set; }
            public SalesRepresentativeComission PortfolioCommission { get; set; }
            public SalesRepresentativeComission GeneralCommission { get; set; }
        }
    }
}

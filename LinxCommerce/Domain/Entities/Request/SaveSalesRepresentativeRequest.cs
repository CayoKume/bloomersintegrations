namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class SaveSalesRepresentativeRequest
    {
        public int SalesRepresentativeID { get; set; }
        public int SourceWebSiteID { get; set; }
        public SaveSalesRepresentativePhotoRequest Photo { get; set; } //obj
        public bool RemovePhoto { get; set; }
        public bool RemoveNotInformedAddresses { get; set; }
        public bool RemoveNotInformedCustomers { get; set; }
        public bool RemoveNotInformedUsers { get; set; }
        public SalesRepresentativePortfolio Portfolio { get; set; } //obj
        public SalesRepresentativeShippingRegion ShippingRegion { get; set; } //obj
        public SalesRepresentativeWebSiteSettings WebSiteSettings { get; set; } //obj
        public SalesRepresentativeAddress Addresses { get; set; } //list obj
        public List<int> UserIDs { get; set; } //list obj
        public string Status { get; set; }
        public string SalesRepresentativeType { get; set; }
        public string Name { get; set; }
        public SalesRepresentativeIdentification Identification { get; set; } //obj
        public string FriendlyCode { get; set; }
        public string IntegrationID { get; set; }
        public SalesRepresentativeContactData Contact { get; set; } //obj
        public List<int> OrderTypeItems { get; set; } //list obj
        public List<int> CompetenceApproversList { get; set; } //list obj
        public bool AllowQuoteDeletion { get; set; }
        public SalesRepresentativeMaxDiscount MaxDiscount { get; set; } //obj
        public SalesRepresentativeComission PortfolioCommission { get; set; } //obj
        public SalesRepresentativeComission GeneralCommission { get; set; } //obj
    }
}

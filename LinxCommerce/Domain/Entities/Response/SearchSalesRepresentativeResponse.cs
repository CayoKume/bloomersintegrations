namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class SearchSalesRepresentativeResponse
    {
        public class Root
        {
            public List<Result> Result { get; set; }
        }

        public class Result
        {
            public bool HasPortfolio { get; set; }
            public string ShippingRegionSelectedMode { get; set; }
            public string ImageUrl { get; set; }
            public int SalesRepresentativeID { get; set; }
            public string Status { get; set; }
            public string SalesRepresentativeType { get; set; }
            public string Name { get; set; }
            public SalesRepresentativeIdentification Identification { get; set; } //obj
            public string FriendlyCode { get; set; }
            public string IntegrationID { get; set; }
            public SalesRepresentativeContactData Contact { get; set; } //obj
            public List<int> OrderTypeItems { get; set; } //obj
            public List<int> CompetenceApproversList { get; set; } //obj
            public bool AllowQuoteDeletion { get; set; }
            //public int BusinessContractID { get; set; }
            public SalesRepresentativeMaxDiscount MaxDiscount { get; set; } //obj
            public SalesRepresentativeComission PortfolioCommission { get; set; } //obj
            public SalesRepresentativeComission GeneralCommission { get; set; } //obj
        }
    }
}

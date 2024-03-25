namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class SearchSKUResponse
    {
        public class Root
        {
            public List<Result> Result { get; set; }
        }

        public class Result
        {
            public string ProductID { get; set; }
            public string Name { get; set; }
            public string SKU { get; set; }
            public string IntegrationID { get; set; }
            public string CreatedDate { get; set; }
            public string ModifiedDate { get; set; }
        }
    }
}

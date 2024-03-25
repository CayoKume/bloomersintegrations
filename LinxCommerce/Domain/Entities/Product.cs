namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class Product
    {
        public List<GetProductResponse.Root> GetProductResponse { get; set; }
        public SearchProductResponse.Root SearchProductResponse { get; set; }
    }
}

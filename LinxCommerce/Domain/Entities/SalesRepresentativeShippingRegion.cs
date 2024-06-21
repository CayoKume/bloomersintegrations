namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class SalesRepresentativeShippingRegion
    {
        public string SelectedMode { get; set; }
        public int ShippingRegionID { get; set; }
        public List<int> PointOfSalesList { get; set; } //list int
    }
}

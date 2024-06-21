namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class SalesRepresentativeWebSiteSettings
    {
        public string WebSiteFilter { get; set; }
        public List<int> WebSiteGroups { get; set; } //list de int
        public List<int> WebSites { get; set; } //list de int
    }
}

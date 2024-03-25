namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class Produto
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string IntegrationID { get; set; }
        public string CatalogItemTypeID { get; set; }
        public string PageTitle { get; set; }
        public string UrlFriendly { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string SearchKeywords { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string MaterializedFullText { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public List<MetadataValue> MetadataValues { get; set; }
    }
}

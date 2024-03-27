namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class Product
    {
        public List<Midia> Medias { get; set; }
        public List<MetadataValue> MetadataValues { get; set; }
        public List<int> TagsID { get; set; }
        public List<int> SkusID { get; set; }
        public List<int> ShippingRegionsID { get; set; }
        public List<int> FlagsID { get; set; }
        public List<int> MediasID { get; set; }
        public List<int> CategoriesID { get; set; }
        public string BrandID { get; set; }
        public string RatingSetID { get; set; }
        public string CatalogItemType { get; set; }
        public string PurchasingPolicyID { get; set; }
        public string IsFreeShipping { get; set; }
        public string SearchKeywords { get; set; }
        public string PageTitle { get; set; }
        public string UrlFriendly { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string WarrantyDescription { get; set; }
        public string IsVisible { get; set; }
        public string VisibleFrom { get; set; }
        public string VisibleTo { get; set; }
        public string IsSearchable { get; set; }
        public string IsUponRequest { get; set; }
        public string DisplayAvailability { get; set; }
        public string DisplayStockQty { get; set; }
        public string DisplayPrice { get; set; }
        public string IsNew { get; set; }
        public string NewFrom { get; set; }
        public string NewTo { get; set; }
        public string UseAcceptanceTerm { get; set; }
        public string AcceptanceTermID { get; set; }
        public string DefinitionID { get; set; }
        public string IsDeleted { get; set; }
        public string SendToMarketplace { get; set; }
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string IntegrationID { get; set; }
    }
}

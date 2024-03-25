namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class Tag
    {
        public string TagID { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string IsSystem { get; set; }
        public string IsDeleted { get; set; }
    }
}

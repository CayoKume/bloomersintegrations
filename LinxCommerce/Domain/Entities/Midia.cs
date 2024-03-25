namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class Midia
    {
        public string MediaID { get; set; }
        public string IsDeleted { get; set; }
        public string Index { get; set; }
        public string CreatedDate { get; set; }
        public string Type { get; set; }
        public string ParentMediaID { get; set; }
        public string OriginalFileName { get; set; }
        public List<MediaAssociation> MediaAssociations { get; set; }
        public Image Image { get; set; }
        public Video Video { get; set; }
    }
}

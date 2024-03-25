namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class Image
    {
        public string RelativePath { get; set; }
        public string Extension { get; set; }
        public string MaxWidth { get; set; }
        public string MaxHeight { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string MediaSizeType { get; set; }
        public string AbsolutePath { get; set; }
    }
}

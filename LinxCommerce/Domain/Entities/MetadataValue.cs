namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class MetadataValue
    {
        public string PropertyMetadataID { get; set; }
        public string PropertyName { get; set; }
        public string PropertyGroup { get; set; }
        public string InputType { get; set; }
        public string Value { get; set; }
        public string SerializedValue { get; set; }
        public string SerializedBlobValue { get; set; }
        public string IntegrationID { get; set; }
        public string FormattedValue { get; set; }
        public string DisplayName { get; set; }
    }
}

namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class OrderAddress
    {
        public string OrderAddressID { get; set; }
        public string OrderID { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Neighbourhood { get; set; }
        public string Number { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string AddressNotes { get; set; }
        public string Landmark { get; set; }
        public string ContactName { get; set; }
        public string ContactDocumentNumber { get; set; }
        public string AddressType { get; set; }
        public string PointOfSaleID { get; set; }
        public string ContactPhone { get; set; }
    }
}

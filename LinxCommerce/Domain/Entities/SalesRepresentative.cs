namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class SalesRepresentative
    {
        public string SalesRepresentativeID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FriendlyCode { get; set; }
        public string MaxDiscountAmount { get; set; }
        public string ImageTimestamp { get; set; }
        public string CellPhone { get; set; }
        public string IntegrationID { get; set; }
        public Identification Identification { get; set; } = new Identification();
        public Commission Commission { get; set; } = new Commission();
    }

    public class Identification
    {
        public string Type { get; set; }
        public string DocumentNumber { get; set; }
    }

    public class Commission
    {
        public string OrderCommission { get; set; }
        public string DeliveryCommission { get; set; }
        public string FromPortfolio { get; set; }
    }
}

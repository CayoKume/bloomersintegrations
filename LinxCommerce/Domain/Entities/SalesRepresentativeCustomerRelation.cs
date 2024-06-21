namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class SalesRepresentativeCustomerRelation
    {
        public int CustomerID { get; set; }
        public SalesRepresentativeMaxDiscount MaxDiscount { get; set; } //obj
        public string Status { get; set; }
        public bool IsMaxDiscountEnabled { get; set; }
    }
}

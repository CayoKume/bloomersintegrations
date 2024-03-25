namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class OrderType
    {
        public string OrderTypeID { get; set; }
        public string Name { get; set; }
        public string RequireInventory { get; set; }
        public string RequirePayment { get; set; }
        public string AllowMultiPayment { get; set; }
        public string EmitFiscalTicket { get; set; }
        public string stringegrationID { get; set; }
    }
}

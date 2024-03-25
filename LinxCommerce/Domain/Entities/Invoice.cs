namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class Invoice
    {
        public string OrderInvoiceID { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        public string FulfillmentID { get; set; }
        public string IsIssued { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public string CFOP { get; set; }
        public string XML { get; set; }
        public string InvoicePdf { get; set; }
        public string Observation { get; set; }
        public string Operation { get; set; }
        public string ProcessedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string IssuedAt { get; set; }
        public string CreatedAt { get; set; }
        public string ID { get; set; }
    }
}

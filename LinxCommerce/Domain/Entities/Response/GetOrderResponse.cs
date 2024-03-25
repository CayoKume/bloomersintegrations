namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class GetOrderResponse
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public class Root
        {
            public string OrderID { get; set; }
            public string OrderNumber { get; set; }
            public string MarketPlaceBrand { get; set; }
            public string OriginalOrderID { get; set; }
            public string WebSiteID { get; set; }
            public string WebSiteIntegrationID { get; set; }
            public string CustomerID { get; set; }
            public string ShopperTicketID { get; set; }
            public string ItemsQty { get; set; }
            public string ItemsCount { get; set; }
            public string TaxAmount { get; set; }
            public string DeliveryAmount { get; set; }
            public string DiscountAmount { get; set; }
            public string PaymentTaxAmount { get; set; }
            public string SubTotal { get; set; }
            public string Total { get; set; }
            public string TotalDue { get; set; }
            public string TotalPaid { get; set; }
            public string TotalRefunded { get; set; }
            public string PaymentDate { get; set; }
            public string PaymentStatus { get; set; }
            public string ShipmentDate { get; set; }
            public string ShipmentStatus { get; set; }
            public string GlobalStatus { get; set; }
            public string DeliveryPostalCode { get; set; }
            public string CreatedChannel { get; set; }
            public string TrafficSourceID { get; set; }
            public string OrderStatusID { get; set; }
            public List<Item> Items { get; set; }
            public List<Tag> Tags { get; set; }
            public List<Property> Properties { get; set; }
            public List<OrderAddress> Addresses { get; set; }
            public List<PaymentMethod> PaymentMethods { get; set; }
            public List<DeliveryMethod> DeliveryMethods { get; set; }
            public List<Discount> Discounts { get; set; }
            public List<Shipment> Shipments { get; set; }
            public List<Fulfillment> Fulfillments { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string ModifiedDate { get; set; }
            public string ModifiedBy { get; set; }
            public string Remarks { get; set; }
            public Seller Seller { get; set; }
            public string SellerCommissionAmount { get; set; }
            public SalesRepresentative SalesRepresentative { get; set; }
            public string CommissionAmount { get; set; }
            public OrderType OrderType { get; set; }
            public Invoice OrderInvoice { get; set; }
            public string OrderGroupID { get; set; }
            public string OrderGroupNumber { get; set; }
            public string HasConflicts { get; set; }
            public string AcquiredDate { get; set; }
            public ExternalInfo ExternalInfo { get; set; }
            public string HasHubOrderWithoutShipmentConflict { get; set; }
            public string CustomerType { get; set; }
            public MultiSiteTenant MultiSiteTenant { get; set; }
            public string CancelledDate { get; set; }
            public Wishlist Wishlist { get; set; }
            public string WebSiteName { get; set; }
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public string CustomerGender { get; set; }
            public string CustomerBirthDate { get; set; }
            public string CustomerPhone { get; set; }
            public string CustomerCPF { get; set; }
            public string CustomerCNPJ { get; set; }
            public string CustomerTradingName { get; set; }
            public string CustomerSiteTaxPayer { get; set; }
        }
    }
}

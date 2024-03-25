namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class SearchOrderResponse
    {
        public class Root
        {
            public List<Result> Result { get; set; }
        }

        public class Result
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
            public List<Item> Items { get; set; } = new List<Item>();
            public List<Tag> Tags { get; set; } = new List<Tag>();
            public List<Property> Properties { get; set; } = new List<Property>();
            public List<OrderAddress> Addresses { get; set; } = new List<OrderAddress>();
            public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
            public List<DeliveryMethod> DeliveryMethods { get; set; } = new List<DeliveryMethod>();
            public List<Discount> Discounts { get; set; } = new List<Discount> ();
            public List<Shipment> Shipments { get; set; } = new List<Shipment>();
            public List<Fulfillment> Fulfillments { get; set; } = new List<Fulfillment>();
            public string CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string ModifiedDate { get; set; }
            public string ModifiedBy { get; set; }
            public string Remarks { get; set; }
            public Seller Seller { get; set; } = new Seller();
            public string SellerCommissionAmount { get; set; }
            public SalesRepresentative SalesRepresentative { get; set; } = new SalesRepresentative();
            public string CommissionAmount { get; set; }
            public OrderType OrderType { get; set; } = new OrderType();
            public Invoice OrderInvoice { get; set; } = new Invoice();
            public string OrderGroupID { get; set; }
            public string OrderGroupNumber { get; set; }
            public string HasConflicts { get; set; }
            public string AcquiredDate { get; set; }
            public ExternalInfo ExternalInfo { get; set; } = new ExternalInfo();
            public string HasHubOrderWithoutShipmentConflict { get; set; }
            public string CustomerType { get; set; }
            public MultiSiteTenant MultiSiteTenant { get; set; } = new MultiSiteTenant();
            public string CancelledDate { get; set; }
            public Wishlist Wishlist { get; set; } = new Wishlist();
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

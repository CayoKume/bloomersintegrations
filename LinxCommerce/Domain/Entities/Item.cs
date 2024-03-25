namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class Item
    {
        public string OrderItemID { get; set; }
        public string OrderID { get; set; }
        public string ParentItemID { get; set; }
        public string ProductID { get; set; }
        public string SkuID { get; set; }
        public string SKU { get; set; }
        public string SellerSKU { get; set; }
        public string WebSiteID { get; set; }
        public string CatalogID { get; set; }
        public string PriceListID { get; set; }
        public string WareHouseID { get; set; }
        public string WarehouseIntegrationID { get; set; }
        public List<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
        public string Qty { get; set; }
        public string Price { get; set; }
        public string DiscountAmount { get; set; }
        public string TaxationAmount { get; set; }
        public string Subtotal { get; set; }
        public string Total { get; set; }
        public string IsFreeShipping { get; set; }
        public string IsDeleted { get; set; }
        public string Status { get; set; }
        public string ProductIntegrationID { get; set; }
        public string SKUIntegrationID { get; set; }
        public string CatalogItemType { get; set; }
        public string IsFreeOffer { get; set; }
        public string IsGiftWrapping { get; set; }
        public string IsService { get; set; }
        public string SpecialType { get; set; }
        public List<Property> Properties { get; set; } = new List<Property>();
        public List<FormData> FormData { get; set; } = new List<FormData>();
        public string BundlePriceType { get; set; }
        public string BundleKitDiscount { get; set; }
        public string BundleKitDiscountValue { get; set; }
        public string InStockHandlingDays { get; set; }
        public string OutStockHandlingDays { get; set; }
        public string ProductName { get; set; }
        public string SkuName { get; set; }
        public string IsDeliverable { get; set; }
        public List<SerialKey> SerialKey { get; set; } = new List<SerialKey>();
        public string Weight { get; set; }
        public string Depth { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public ExternalInfo ExternalInfo { get; set; } = new ExternalInfo();
        public string FulfillmentID { get; set; }
        public string UPC { get; set; }
        public string OrderPackageItemID { get; set; }
        public string OrderPackageID { get; set; }
        public string Quantity { get; set; }
    }

    public class FormData
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string DisplayName { get; set; }
    }

    public class SerialKey
    {
        public string SerialKeyID { get; set; }
        public string OrderItemID { get; set; }
        public string Serial { get; set; }
    }

    public class Warehouse
    {
        public string WarehouseID { get; set; }
        public string WarehouseIntegrationID { get; set; }
        public string DockID { get; set; }
        public string Quantity { get; set; }
    }

    public class ExternalInfo
    {
        public string OrderItemExternalID { get; set; }
        public string OrderExternalID { get; set; }
        public string ProductType { get; set; }
        public string IntegrationOrderID { get; set; }
        public string OrderStatus { get; set; }
        public string DeliveryLogisticType { get; set; }
        public string ShippingLabelStatus { get; set; }
        public string IsProcessing { get; set; }
        public string MarketPlaceCnpj { get; set; }
        public string SellerCnpj { get; set; }
        public string SellerName { get; set; }
    }
}

namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class Shipment
    {
        public string OrderShipmentID { get; set; }
        public string OrderID { get; set; }
        public string DeliveryMethodID { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipmentStatus { get; set; }
        public string AssignUserId { get; set; }
        public string AssignUserName { get; set; }
        public string DockID { get; set; }
        public List<Package> Packages { get; set; } = new List<Package>();
    }

    public class Package
    {
        public string OrderPackageID { get; set; }
        public string OrderShipmentID { get; set; }
        public string DeliveryMethodID { get; set; }
        public string PackageNumber { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingNumberUrl { get; set; }
        public string ShippedDate { get; set; }
        public string ShippedBy { get; set; }
        public string IsDeleted { get; set; }
        public string PackageType { get; set; }
        public string Source { get; set; }
        public string InsuranceAmount { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Length { get; set; }
        public string Weight { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}

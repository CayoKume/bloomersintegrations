namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class DeliveryMethod
    {
        public string LogisticOptionId { get; set; }
        public string LogisticOptionName { get; set; }
        public string LogisticContractId { get; set; }
        public string LogisticContractName { get; set; }
        public string OrderDeliveryMethodID { get; set; }
        public string OrderID { get; set; }
        public string DeliveryMethodID { get; set; }
        public string DeliveryGroupID { get; set; }
        public string Amount { get; set; }
        public string ETA { get; set; }
        public string ETADays { get; set; }
        public string IntegrationID { get; set; }
        public string ScheduleShiftID { get; set; }
        public string ScheduleDisplayName { get; set; }
        public string ScheduleTax { get; set; }
        //public ScheduleStartTime ScheduleStartTime { get; set; }
        //public ScheduleEndTime ScheduleEndTime { get; set; }
        public string ScheduleDate { get; set; }
        public string DeliveryMethodAlias { get; set; }
        public string PointOfSaleID { get; set; }
        public string PointOfSaleIntegrationID { get; set; }
        public string PointOfSaleName { get; set; }
        public string DeliveryMethodType { get; set; }
        public string ExternalID { get; set; }
        public string WarehouseID { get; set; }
        public string WarehouseIntegrationID { get; set; }
        public string DockID { get; set; }
        //public List<ExtendedProperty> ExtendedProperties { get; set; }
        public string CarrierName { get; set; }
        public string DeliveryEstimatedDate { get; set; }
    }
}

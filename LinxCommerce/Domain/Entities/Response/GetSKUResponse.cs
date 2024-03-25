namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class GetSKUResponse
    {
        public class Result
        {
            public List<Root> _Result { get; set; }
        }

        public class Root
        {  
            public string UPC { get; set; }
            public string DisplayCondition { get; set; }
            public string DefinitionID { get; set; }
            public string ConditionID { get; set; }
            public string UnitOfMeasureID { get; set; }
            public string ManageStock { get; set; }
            public string MinimumQtyAllowed { get; set; }
            public string MaximumQtyAllowed { get; set; }
            public string Preorderable { get; set; }
            public string PreorderDate { get; set; }
            public string PreorderLimit { get; set; }
            public string Backorderable { get; set; }
            public string BackorderLimit { get; set; }
            public string PurchasingPolicyID { get; set; }
            public string WrappingQty { get; set; }
            public string Weight { get; set; }
            public string Height { get; set; }
            public string Width { get; set; }
            public string Depth { get; set; }
            public string Cost { get; set; }
            public string Tax { get; set; }
            public string IsVisible { get; set; }
            public string VisibleFrom { get; set; }
            public string VisibleTo { get; set; }
            public string IsDeleted { get; set; }
            public string ProductID { get; set; }
            public string Name { get; set; }
            public string SKU { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string ModifiedDate { get; set; }
            public string ModifiedBy { get; set; }
            public string IntegrationID { get; set; }

            public List<string> SupplierID { get; set; }
            public List<string> ParentsID { get; set; }
            public List<MetadataValue>  MetadataValues { get; set; }
            public List<ParentRelation> ParentRelations { get; set; }
        }
    }
}

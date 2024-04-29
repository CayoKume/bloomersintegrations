namespace NewBloomersWebApplication.Domain.Entities.Picking
{
    public class Product : BloomersIntegrationsCore.Domain.Entities.Product
    {
        public string urlImg { get; set; }
        public double quantityPicked { get; set; }
    }
}

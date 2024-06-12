namespace BloomersMiniWmsIntegrations.Domain.Entities.Picking
{
    public class Product : BloomersIntegrationsCore.Domain.Entities.Product
    {
        public string idItem { get; set; }
        public string urlImg { get; set; }
        public double picked_quantity { get; set; }
    }
}

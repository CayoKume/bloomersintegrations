namespace BloomersMiniWmsIntegrations.Domain.Entities.Picking
{
    public class Product : BloomersIntegrationsCore.Domain.Entities.Product
    {
        public string urlImg { get; set; }
        public double quantidade_conferida { get; set; }
    }
}

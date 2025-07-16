namespace NewBloomersWebApplication.Infrastructure.Domain.Entities.CancellationRequest
{
    public class ProductToCancellation : BloomersIntegrationsCore.Domain.Entities.Product
    {
        public int picked_quantity_product { get; set; }
    }
}

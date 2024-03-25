namespace BloomersCarriersIntegrations.FlashCourier.Domain.Entities
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        public float weight { get; set; }
        public string quantity { get; set; }
    }
}

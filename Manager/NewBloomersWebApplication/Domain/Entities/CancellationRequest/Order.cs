namespace NewBloomersWebApplication.Domain.Entities.CancellationRequest
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        public string requester { get; set; }
        public int reason { get; set; }
        public int picked_quantity { get; set; }
    }
}

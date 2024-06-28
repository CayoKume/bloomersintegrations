namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class OrderToCancellationRequest
    {
        public OrderToCancellation serializeOrder { get; set; }
    }

    public class OrderToCancellation
    {
        private List<BloomersMiniWmsIntegrations.Domain.Entities.CancellationRequest.ProductToCancellation> _itens = new List<BloomersMiniWmsIntegrations.Domain.Entities.CancellationRequest.ProductToCancellation>();

        public string requester { get; set; }
        public int reason { get; set; }
        public int picked_quantity { get; set; }

        public List<BloomersMiniWmsIntegrations.Domain.Entities.CancellationRequest.ProductToCancellation> itens { get { return _itens; } set { _itens = value; } }
    }
}

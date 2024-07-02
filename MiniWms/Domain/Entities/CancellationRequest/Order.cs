namespace BloomersMiniWmsIntegrations.Domain.Entities.CancellationRequest
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<BloomersMiniWmsIntegrations.Domain.Entities.CancellationRequest.ProductToCancellation> _itens = new List<BloomersMiniWmsIntegrations.Domain.Entities.CancellationRequest.ProductToCancellation>();

        public string requester { get; set; }
        public int reason { get; set; }

        public List<BloomersMiniWmsIntegrations.Domain.Entities.CancellationRequest.ProductToCancellation> itens { get { return _itens; } set { _itens = value; } }
    }
}

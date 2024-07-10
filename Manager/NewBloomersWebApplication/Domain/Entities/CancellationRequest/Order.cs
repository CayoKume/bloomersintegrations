namespace NewBloomersWebApplication.Domain.Entities.CancellationRequest
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<NewBloomersWebApplication.Domain.Entities.CancellationRequest.ProductToCancellation> _itens = new List<NewBloomersWebApplication.Domain.Entities.CancellationRequest.ProductToCancellation>();
        public string requester { get; set; }
        public int reason { get; set; }
        public string obs { get; set; }
        public List<NewBloomersWebApplication.Domain.Entities.CancellationRequest.ProductToCancellation> itens { get { return _itens; } set { _itens = value; } }
    }
}

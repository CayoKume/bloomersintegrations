namespace NewBloomersWebApplication.Infrastructure.Domain.Entities.CancellationRequest
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<ProductToCancellation> _itens = new List<ProductToCancellation>();
        public string requester { get; set; }
        public int reason { get; set; }
        public string obs { get; set; }
        public List<ProductToCancellation> itens { get { return _itens; } set { _itens = value; } }
    }
}

using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class PrintOrderRequest
    {
        public Order serializePedido { get; set; }
    }

    public class Order
    {
        private List<BloomersMiniWmsIntegrations.Domain.Entities.Picking.ProductToPrint> _itens = new List<BloomersMiniWmsIntegrations.Domain.Entities.Picking.ProductToPrint>();

        public string number { get; set; }
        public string? obs { get; set; }
        public string? seller { get; set; }
        public string? amount { get; set; }

        public Client? client { get; set; }
        public ShippingCompany? shippingCompany { get; set; }
        public Company company { get; set; }
        public List<BloomersMiniWmsIntegrations.Domain.Entities.Picking.ProductToPrint> itens { get { return _itens; } set { _itens = value; } }
    }
}

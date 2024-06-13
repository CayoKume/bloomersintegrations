using BloomersIntegrationsCore.Domain.Entities;

namespace NewBloomersWebApplication.Domain.Entities.Picking
{
    public class OrderToPrint
    {
        private List<ProductToPrint> _itens = new List<ProductToPrint>();

        public string number { get; set; }
        public string? obs { get; set; }
        public string? seller { get; set; }
        public string? amount { get; set; }

        public Client? client { get; set; }
        public ShippingCompany? shippingCompany { get; set; }
        public Company company { get; set; }

        public List<ProductToPrint> itens { get { return _itens; } set { _itens = value; } }
    }
}

using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersCarriersIntegrations.TotalExpress.Domain.Entities
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<Product> _items = new List<Product>();

        public int SERVICOTIPO { get; set; }
        public string REMETENTEID { get; set; }

        public Client? client { get; set; }
        public Company? company { get; set; }
        public ShippingCompany? shippingCompany { get; set; }
        public Invoice? invoice { get; set; }
        public List<Product> items { get { return _items; } set { _items = value; } }

    }
}

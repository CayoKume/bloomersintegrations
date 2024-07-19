namespace BloomersCarriersIntegrations.Jadlog.Domain.Entities
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<Product> _itens = new List<Product>();

        public List<Product> itens { get { return _itens; } set { _itens = value; } }
    }
}

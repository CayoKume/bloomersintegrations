using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersCarriersIntegrations.Jadlog.Domain.Entities
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<Product> _itens = new List<Product>();

        public string TIPO_SERVICO { get; set; }
        public Company tomador { get; set; }

        public List<Product> itens { get { return _itens; } set { _itens = value; } }
    }
}

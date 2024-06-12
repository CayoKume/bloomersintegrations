using NewBloomersWebApplication.Domain.Entities.Picking;

namespace NewBloomersWebApplication.Domain.Entities.Picking
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<Product> _itens = new List<Product>();

        public string? buttonText { get; set; }
        public string? buttonClass { get; set; }

        public string? obs { get; set; }
        public string? seller { get; set; }

        public DateTime? retorno { get; set; }

        public List<Product> itens { get { return _itens; } set { _itens = value; } }
    }
}

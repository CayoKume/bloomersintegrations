using BloomersIntegrationsCore.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class PrintOrderRequest
    {
        [Required(ErrorMessage = "O Pedido é Obrigatório")]
        public Order serializePedido { get; set; }
    }

    public class Order
    {
        private List<BloomersIntegrationsManager.Domain.Entities.MiniWms.Product> _itens = new List<BloomersIntegrationsManager.Domain.Entities.MiniWms.Product>();

        public string number { get; set; }
        public string? obs { get; set; }
        public string? seller { get; set; }
        public string? amount { get; set; }

        public Client? client { get; set; }
        public ShippingCompany? shippingCompany { get; set; }
        public Company company { get; set; }
        public List<BloomersIntegrationsManager.Domain.Entities.MiniWms.Product> itens { get { return _itens; } set { _itens = value; } }
    }

    public class Product : BloomersIntegrationsCore.Domain.Entities.Product
    {
        public string idItem { get; set; }
    }
}

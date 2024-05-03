using BloomersIntegrationsCore.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class PrintOrderRequest
    {
        [Required(ErrorMessage = "A Lista de Pedidos é Obrigatória")]
        public List<Order> serializePedidosList { get; set; }
    }

    public class Order
    {
        public string number { get; set; }
        public Client? client { get; set; }
        public Invoice? invoice { get; set; }
        public ShippingCompany? shippingCompany { get; set; }
        public Company company { get; set; }
    }
}

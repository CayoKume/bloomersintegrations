using BloomersIntegrationsCore.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class PrintOrdersRequest
    {
        [Required(ErrorMessage = "A Lista de Pedidos é Obrigatória")]
        public List<Orders> serializePedidosList { get; set; }
    }

    public class Orders
    {
        public string number { get; set; }
        public Client? client { get; set; }
        public Invoice? invoice { get; set; }
        public ShippingCompany? shippingCompany { get; set; }
        public Company company { get; set; }
    }
}

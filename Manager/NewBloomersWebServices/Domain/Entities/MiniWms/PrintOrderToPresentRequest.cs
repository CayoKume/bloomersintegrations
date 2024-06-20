using BloomersIntegrationsCore.Domain.Entities;

namespace NewBloomersWebServices.Domain.Entities.MiniWms
{
    public class PrintOrderToPresentRequest
    {
        public OrderToPresent serializePedido { get; set; }
    }

    public class OrderToPresent
    {
        public string number { get; set; }
        public string? token { get; set; }

        public Client? client { get; set; }
        public Company company { get; set; }
        public Invoice invoice { get; set; }
    }
}

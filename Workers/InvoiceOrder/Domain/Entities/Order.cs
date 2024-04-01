namespace BloomersWorkers.InvoiceOrder.Domain.Entities
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        public int invoice_attempts { get; set; }
        public string volumes { get; set; }
    }
}

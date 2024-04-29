namespace BloomersWorkers.InvoiceOrder.Application.Services
{
    public interface IInvoiceOrderService
    {
        public Task InvoiceOrder();
        public Task InvoiceOrder(string workerName);
    }
}

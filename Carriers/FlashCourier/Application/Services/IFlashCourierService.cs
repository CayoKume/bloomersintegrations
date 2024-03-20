namespace BloomersCarriersIntegrations.FlashCourier.Application.Services
{
    public interface IFlashCourierService
    {
        public Task<bool> EnviaPedidosFlash();
        public Task<bool> EnviaPedidoFlash(string order_number);
        public Task AtualizaLogPedidoEnviado();
    }
}

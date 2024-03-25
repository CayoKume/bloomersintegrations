using BloomersCarriersIntegrations.FlashCourier.Domain.Entities;
using BloomersIntegrationsCore.Domain.Entities;
using Order = BloomersCarriersIntegrations.FlashCourier.Domain.Entities.Order;

namespace BloomersCarriersIntegrations.FlashCourier.Infrastructure.Repositorys
{
    public interface IFlashCourierRepository
    {
        public Task<Order> GetInvoicedOrder(string orderNumber);
        public Task<IEnumerable<Order>> GetInvoicedOrders();
        public Task<IEnumerable<FlashCourierRegisterLog>> GetShippedOrders();
        public Task<HAWBRequest> GetAWBRequest(string cnpj);
        public Task<FlashCourierParameters> GetAuthenticationUser(string cnpj);
        public Task GenerateSucessLog(string orderNumber, string senderID, string _return, string statusFlash, string keyNFe);
        public Task UpdateCollectionDate(string dtSla, string cardCode);
        public Task UpdateRealDeliveryForecastDate(string dtSla, string cardCode);
        public Task UpdateDeliveryMadeDate(string occurrence, string cardCode);
        public Task UpdateLastStatusDate(string occurrence, string eventId, string _event, string cardCode);
    }
}

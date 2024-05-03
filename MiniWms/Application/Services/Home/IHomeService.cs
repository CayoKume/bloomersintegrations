using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public interface IHomeService
    {
        public Task<string> GetPickupOrders(string doc_company);
    }
}

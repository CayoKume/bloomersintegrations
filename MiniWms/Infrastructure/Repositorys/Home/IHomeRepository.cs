using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface IHomeRepository
    {
        public Task<IEnumerable<Order>?> GetPickupOrders(string doc_company);
    }
}

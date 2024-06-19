using BloomersMiniWmsIntegrations.Domain.Entities.Home;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface IHomeRepository
    {
        public Task<IEnumerable<Order>?> GetPickupOrders(string doc_company);
    }
}

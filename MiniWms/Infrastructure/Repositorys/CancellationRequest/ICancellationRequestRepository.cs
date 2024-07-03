using BloomersMiniWmsIntegrations.Domain.Entities.CancellationRequest;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface ICancellationRequestRepository
    {
        public Task<bool> CreateCancellationRequest(Order order);
        public Task<Dictionary<int, string>> GetReasons();
        public Task<Order> GetOrderToCancel(string number);
    }
}

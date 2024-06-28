using BloomersMiniWmsIntegrations.Domain.Entities.CancellationRequest;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface ICancellationRequestRepository
    {
        public Task CreateCancellationRequest(Order order);
        public Task<Dictionary<int, string>> GetReasons();
    }
}

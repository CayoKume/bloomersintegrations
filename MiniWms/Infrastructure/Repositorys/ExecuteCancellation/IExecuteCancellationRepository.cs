using BloomersMiniWmsIntegrations.Domain.Entities.ExecuteCancellation;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface IExecuteCancellationRepository
    {
        public Task<Dictionary<int, string>> GetReasons();
        public Task<IEnumerable<Order>> GetOrdersToCancel(string serie, string doc_company);
        public Task<bool> UpdateDateCanceled(string number, string suporte, string inputObs);
    }
}

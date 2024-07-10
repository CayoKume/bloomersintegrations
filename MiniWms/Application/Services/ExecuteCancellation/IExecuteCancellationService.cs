using BloomersMiniWmsIntegrations.Domain.Entities.ExecuteCancellation;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public interface IExecuteCancellationService
    {
        public Task<string> GetReasons();
        public Task<string> GetOrdersToCancel(string serie, string doc_company);
        public Task<bool> UpdateDateCanceled(string number, string suporte, string inputObs);
    }
}

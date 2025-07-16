using NewBloomersWebApplication.Infrastructure.Domain.Entities.ExecuteCancellation;

namespace NewBloomersWebApplication.Application.Services
{
    public interface IExecuteCancellationService
    {
        public Task<Dictionary<int, string>> GetReasons();
        public Task<List<Order>> GetOrdersToCancel(string serie, string doc_company);
        public Task<bool> UpdateDateCanceled(string number, string suporte, string inputObs, int motivo);
    }
}
